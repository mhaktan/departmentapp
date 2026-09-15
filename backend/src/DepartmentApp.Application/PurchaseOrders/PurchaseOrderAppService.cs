using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.PurchaseOrders.Dto;
using DepartmentApp.Analytics.Dto;
using DepartmentApp.StateMachine.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.PurchaseOrders
{
    public class PurchaseOrderAppService : AsyncCrudAppService<
        PurchaseOrder,
        PurchaseOrderDto,
        long,
        PagedPurchaseOrderResultRequestDto,
        CreatePurchaseOrderDto,
        PurchaseOrderDto>,
        IPurchaseOrderAppService
    {
        private readonly IRepository<StatusChangeLog, long> _statusChangeLogRepo;
        private readonly IRepository<ApprovalRecord, Guid> _approvalRepo;
        private readonly IFlowEngine _flowEngine;

        public PurchaseOrderAppService(IRepository<PurchaseOrder, long> repository, IFlowEngine flowEngine, IRepository<StatusChangeLog, long> statusChangeLogRepo, IRepository<ApprovalRecord, Guid> approvalRepo)
            : base(repository)
        {
            _flowEngine = flowEngine;
            _statusChangeLogRepo = statusChangeLogRepo;
            _approvalRepo = approvalRepo;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.PurchaseOrder_Read;
            GetAllPermissionName = PermissionNames.PurchaseOrder_Read;
            CreatePermissionName = PermissionNames.PurchaseOrder_Create;
            UpdatePermissionName = PermissionNames.PurchaseOrder_Update;
            DeletePermissionName = PermissionNames.PurchaseOrder_Delete;
        }

        protected override IQueryable<PurchaseOrder> CreateFilteredQuery(PagedPurchaseOrderResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.OrderNumber != null && x.OrderNumber.Contains(input.Keyword)) ||
                    (x.Notes != null && x.Notes.Contains(input.Keyword)) ||
                    (x.RejectionReason != null && x.RejectionReason.Contains(input.Keyword)))
                .WhereIf(!input.OrderNumber.IsNullOrWhiteSpace(), x => x.OrderNumber != null && x.OrderNumber.Contains(input.OrderNumber))
                .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes != null && x.Notes.Contains(input.Notes))
                .WhereIf(!input.RejectionReason.IsNullOrWhiteSpace(), x => x.RejectionReason != null && x.RejectionReason.Contains(input.RejectionReason))
                .WhereIf(input.OrderDate.HasValue, x => x.OrderDate == input.OrderDate.Value)
                .WhereIf(input.DeliveryDate.HasValue, x => x.DeliveryDate == input.DeliveryDate.Value)
                .WhereIf(input.OrderAmount.HasValue, x => x.OrderAmount == input.OrderAmount.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (PurchaseOrderStatus)input.Status.Value)
                .WhereIf(input.OrderDateFrom.HasValue, x => x.OrderDate >= input.OrderDateFrom.Value)
                .WhereIf(input.OrderDateTo.HasValue, x => x.OrderDate <= input.OrderDateTo.Value)
                .WhereIf(input.DeliveryDateFrom.HasValue, x => x.DeliveryDate >= input.DeliveryDateFrom.Value)
                .WhereIf(input.DeliveryDateTo.HasValue, x => x.DeliveryDate <= input.DeliveryDateTo.Value)
                .WhereIf(input.OrderAmountFrom.HasValue, x => x.OrderAmount >= input.OrderAmountFrom.Value)
                .WhereIf(input.OrderAmountTo.HasValue, x => x.OrderAmount <= input.OrderAmountTo.Value)
                .WhereIf(!input.StatusIn.IsNullOrWhiteSpace(), x => input.StatusIn
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => (PurchaseOrderStatus)int.Parse(v.Trim()))
                    .Contains(x.Status))
                .WhereIf(input.StatusNot.HasValue, x => x.Status != (PurchaseOrderStatus)input.StatusNot.Value)
                .WhereIf(input.PurchaseRequestId.HasValue, x => x.PurchaseRequestId == input.PurchaseRequestId.Value)
                .WhereIf(input.SupplierId.HasValue, x => x.SupplierId == input.SupplierId.Value);
        }

        public override async Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "PurchaseOrder", result);

            // Frontend creates records with status pre-set without going through ChangeStatusAsync,
            // so mirror on-field-change here whenever the initial status isn't the default. Otherwise
            // status-driven flows (e.g. approval) never fire on plain Create.
            if (result.Status != (int)PurchaseOrderStatus.Draft)
                await _flowEngine.TriggerAsync("on-field-change", "PurchaseOrder", result);
            return result;
        }

        public override async Task<PurchaseOrderDto> UpdateAsync(PurchaseOrderDto input)
        {
            // State machine: validate status transition + log
            var existing = await Repository.GetAsync(input.Id);
            var statusChanged = (int)existing.Status != input.Status;
            if (statusChanged)
            {
                var fromStatus = existing.Status.ToString();
                var toStatus = ((PurchaseOrderStatus)input.Status).ToString();
                ValidateStatusTransition(existing.Status, (PurchaseOrderStatus)input.Status);

                // Log status change
                await _statusChangeLogRepo.InsertAsync(new StatusChangeLog
                {
                    EntityType = "PurchaseOrder",
                    EntityId = input.Id.ToString(),
                    FromStatus = fromStatus,
                    ToStatus = toStatus,
                    Action = "Update",
                    ChangedByUserId = AbpSession.UserId
                });
            }

            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "PurchaseOrder", result);

            // Frontend updates status via plain UpdateAsync (not ChangeStatusAsync) — fire
            // on-field-change so status-driven flows pick up the transition.
            if (statusChanged)
                await _flowEngine.TriggerAsync("on-field-change", "PurchaseOrder", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "PurchaseOrder", new { Id = input.Id });
        }

        // Onay adimini tamamlayan rol genelde Update degil ChangeStatus yetkisine sahip olur;
        // RequireAllPermissions=false ile ikisinden biri yeterli (geriye donuk uyumlu).
        [Abp.Authorization.AbpAuthorize(PermissionNames.PurchaseOrder_ChangeStatus, PermissionNames.PurchaseOrder_Update, RequireAllPermissions = false)]
        public async Task<PurchaseOrderDto> ChangeStatusAsync(long id, ChangeStatusInput input)
        {
            var entity = await Repository.GetAsync(id);
            var currentStatus = entity.Status.ToString();

            // Find valid transition
            var transitions = new (string From, string To, string Action, bool Readonly)[]
            {
            ("Draft", "PendingApproval", "Submit", false),
            ("PendingApproval", "Approved", "Approve", false),
            ("PendingApproval", "Draft", "Revise", false),
            ("Draft", "Rejected", "Reject", true)
            };

            var transition = transitions.FirstOrDefault(t =>
                (t.From == "*" || t.From == currentStatus) && t.Action == input.Action);

            if (transition == default)
                throw new Abp.UI.UserFriendlyException($"Invalid action '{input.Action}' from status '{currentStatus}'");

            // Onay ekranindan gelen serbest metni gecisin zorunlu alanlarina tasi
            if (input.ActionData == null) input.ActionData = new Dictionary<string, string>();
            var genericNote = input.ActionData.ContainsKey("comment") && !string.IsNullOrWhiteSpace(input.ActionData["comment"])
                ? input.ActionData["comment"]
                : (input.ActionData.ContainsKey("revisionNote") ? input.ActionData["revisionNote"] : null);
            if (!string.IsNullOrWhiteSpace(genericNote))
            {
                foreach (var rf in new[] { "rejectionReason" })
                {
                    if (!input.ActionData.ContainsKey(rf) || string.IsNullOrWhiteSpace(input.ActionData[rf]))
                        input.ActionData[rf] = genericNote;
                }
            }
            // Validate required fields per transition
            if (input.Action == "Revise" && (input.ActionData == null || !input.ActionData.ContainsKey("rejectionReason") || string.IsNullOrWhiteSpace(input.ActionData["rejectionReason"])))
                throw new Abp.UI.UserFriendlyException("Revise requires: rejectionReason");
            if (input.Action == "Reject" && (input.ActionData == null || !input.ActionData.ContainsKey("rejectionReason") || string.IsNullOrWhiteSpace(input.ActionData["rejectionReason"])))
                throw new Abp.UI.UserFriendlyException("Reject requires: rejectionReason");
            // Bu gecisler icin bagli kayit on kosulu yok

            var fromStatus = currentStatus;

            // Apply new status
            entity.Status = (PurchaseOrderStatus)Enum.Parse(typeof(PurchaseOrderStatus), transition.To);
            if (input.ActionData != null && input.ActionData.ContainsKey("rejectionReason") && !string.IsNullOrWhiteSpace(input.ActionData["rejectionReason"]))
                entity.RejectionReason = input.ActionData["rejectionReason"];

            await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Cancel pending ApprovalRecords when the entity is cancelled — otherwise the records
            // sit forever in approvers' inboxes pointing to a cancelled request.
            if (input.Action == "Cancel")
            {
                var pending = _approvalRepo.GetAll()
                    .Where(a => a.EntityType == "PurchaseOrder" && a.EntityId == id.ToString() && a.Status == "Pending")
                    .ToList();
                foreach (var pendingRec in pending)
                {
                    pendingRec.Status = "Cancelled";
                    pendingRec.ActionTaken = "Cancel";
                    pendingRec.ActionDate = DateTime.UtcNow;
                    pendingRec.Comment = "Entity cancelled by submitter.";
                    await _approvalRepo.UpdateAsync(pendingRec);
                }
            }

            // Log status change
            await _statusChangeLogRepo.InsertAsync(new Entities.StatusChangeLog
            {
                EntityType = "PurchaseOrder",
                EntityId = id.ToString(),
                FromStatus = fromStatus,
                ToStatus = transition.To,
                Action = input.Action,
                Comment = input.ActionData != null && input.ActionData.ContainsKey("comment") ? input.ActionData["comment"] : null,
                ChangedByUserId = AbpSession.UserId
            });

            var result = MapToEntityDto(entity);

            // Trigger flow: on-status-change (always)
            await _flowEngine.TriggerAsync("on-field-change", "PurchaseOrder", result);

            // Trigger named flow events
            if (input.Action == "Submit")
                await _flowEngine.TriggerAsync("submit-for-approval", "PurchaseOrder", result);
            return result;
        }

        private void ValidateStatusTransition(PurchaseOrderStatus from, PurchaseOrderStatus to)
        {
            var allowed = new (string From, string To)[]
            {
                ("Draft", "PendingApproval"),
                ("PendingApproval", "Approved"),
                ("PendingApproval", "Draft"),
                ("Draft", "Rejected")
            };

            var isValid = allowed.Any(t =>
                (t.From == "*" || t.From == from.ToString()) &&
                t.To == to.ToString());

            if (!isValid)
                throw new Abp.UI.UserFriendlyException($"Invalid status transition from {from} to {to}");
        }
        [Abp.Authorization.AbpAuthorize(PermissionNames.PurchaseOrder_Read)]
        public List<GroupCountDto> GetGroupedCount(PurchaseOrderGroupedCountInput input)
        {
            // Whitelist — istemciden gelen alan adı doğrudan sorguya girmez.
            var allowed = new[] { "Status", "PurchaseRequestId", "SupplierId" };
            if (input.GroupBy == null || !allowed.Contains(input.GroupBy))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Gruplanabilir alan degil: {input.GroupBy}. Izin verilenler: {string.Join(", ", allowed)}");
            }

            var query = CreateFilteredQuery(input);

            switch (input.GroupBy)
            {
                case "Status":
                    return query
                        .GroupBy(x => x.Status)
                        .Select(g => new GroupCountDto
                        {
                            Key = ((int)g.Key).ToString(),
                            Label = g.Key.ToString(),
                            Count = g.Count(),
                        })
                        .ToList();
                case "PurchaseRequestId":
                    return query
                        .GroupBy(x => new { Key = x.PurchaseRequestId, Label = x.PurchaseRequest == null ? null : x.PurchaseRequest.RequestNumber })
                        .Select(g => new GroupCountDto
                        {
                            Key = g.Key.Key.ToString(),
                            Label = g.Key.Label ?? "(bos)",
                            Count = g.Count(),
                        })
                        .ToList();
                case "SupplierId":
                    return query
                        .GroupBy(x => new { Key = x.SupplierId, Label = x.Supplier == null ? null : x.Supplier.Name })
                        .Select(g => new GroupCountDto
                        {
                            Key = g.Key.Key.ToString(),
                            Label = g.Key.Label ?? "(bos)",
                            Count = g.Count(),
                        })
                        .ToList();
                default:
                    return new List<GroupCountDto>();
            }
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.PurchaseOrder_Read)]
        public decimal? GetStats(PurchaseOrderStatsInput input)
        {
            var query = CreateFilteredQuery(input);

            if (input.Aggregate == "avgDayDiff")
            {
                var allowedDates = new[] { "OrderDate", "DeliveryDate" };
                if (!allowedDates.Contains(input.FromField) || !allowedDates.Contains(input.ToField))
                {
                    throw new Abp.UI.UserFriendlyException("avgDayDiff icin gecerli iki tarih alani gerekli.");
                }
                switch (input.FromField + "|" + input.ToField)
                {
                    case "OrderDate|DeliveryDate":
                    {
                        var pairsOrderDateDeliveryDate = query
                            .Where(x => x.DeliveryDate != null)
                            .Select(x => new { A = x.OrderDate, B = x.DeliveryDate.Value })
                            .ToList();
                        if (pairsOrderDateDeliveryDate.Count == 0) return null;
                        return (decimal)pairsOrderDateDeliveryDate.Average(p => (p.B - p.A).TotalDays);
                    }
                    case "DeliveryDate|OrderDate":
                    {
                        var pairsDeliveryDateOrderDate = query
                            .Where(x => x.DeliveryDate != null)
                            .Select(x => new { A = x.DeliveryDate.Value, B = x.OrderDate })
                            .ToList();
                        if (pairsDeliveryDateOrderDate.Count == 0) return null;
                        return (decimal)pairsDeliveryDateOrderDate.Average(p => (p.B - p.A).TotalDays);
                    }
                    default: return null;
                }
            }

            var allowedNumeric = new[] { "OrderAmount" };
            if (!allowedNumeric.Contains(input.Field))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Toplanabilir alan degil: {input.Field}. Izin verilenler: {string.Join(", ", allowedNumeric)}");
            }
            switch (input.Field)
            {
                        case "OrderAmount": return input.Aggregate == "sum" ? query.Sum(x => (decimal?)x.OrderAmount)
                            : input.Aggregate == "min" ? query.Min(x => (decimal?)x.OrderAmount)
                            : input.Aggregate == "max" ? query.Max(x => (decimal?)x.OrderAmount)
                            : query.Average(x => (decimal?)x.OrderAmount);
                        default: return null;
            }
        }

    }
}
