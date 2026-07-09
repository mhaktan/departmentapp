using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.PeerReviews.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.PeerReviews
{
    public class PeerReviewAppService : AsyncCrudAppService<
        PeerReview,
        PeerReviewDto,
        long,
        PagedPeerReviewResultRequestDto,
        CreatePeerReviewDto,
        PeerReviewDto>,
        IPeerReviewAppService
    {
        private readonly IFlowEngine _flowEngine;

        public PeerReviewAppService(IRepository<PeerReview, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.PeerReview_Read;
            GetAllPermissionName = PermissionNames.PeerReview_Read;
            CreatePermissionName = PermissionNames.PeerReview_Create;
            UpdatePermissionName = PermissionNames.PeerReview_Update;
            DeletePermissionName = PermissionNames.PeerReview_Delete;
        }

        protected override IQueryable<PeerReview> CreateFilteredQuery(PagedPeerReviewResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.ReviewerName != null && x.ReviewerName.Contains(input.Keyword)) ||
                    (x.Strengths != null && x.Strengths.Contains(input.Keyword)) ||
                    (x.Improvements != null && x.Improvements.Contains(input.Keyword)))
                .WhereIf(!input.ReviewerName.IsNullOrWhiteSpace(), x => x.ReviewerName != null && x.ReviewerName.Contains(input.ReviewerName))
                .WhereIf(!input.Strengths.IsNullOrWhiteSpace(), x => x.Strengths != null && x.Strengths.Contains(input.Strengths))
                .WhereIf(!input.Improvements.IsNullOrWhiteSpace(), x => x.Improvements != null && x.Improvements.Contains(input.Improvements))
                .WhereIf(input.Score.HasValue, x => x.Score == input.Score.Value)
                .WhereIf(input.IsAnonymous.HasValue, x => x.IsAnonymous == input.IsAnonymous.Value)
                .WhereIf(input.PerformanceReviewId.HasValue, x => x.PerformanceReviewId == input.PerformanceReviewId.Value)
                .WhereIf(input.EmployeeId.HasValue, x => x.EmployeeId == input.EmployeeId.Value);
        }

        public override async Task<PeerReviewDto> CreateAsync(CreatePeerReviewDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "PeerReview", result);
            return result;
        }

        public override async Task<PeerReviewDto> UpdateAsync(PeerReviewDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "PeerReview", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "PeerReview", new { Id = input.Id });
        }
    }
}
