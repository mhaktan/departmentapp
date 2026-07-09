using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.TrainingParticipations.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.TrainingParticipations
{
    public class TrainingParticipationAppService : AsyncCrudAppService<
        TrainingParticipation,
        TrainingParticipationDto,
        long,
        PagedTrainingParticipationResultRequestDto,
        CreateTrainingParticipationDto,
        TrainingParticipationDto>,
        ITrainingParticipationAppService
    {
        private readonly IFlowEngine _flowEngine;

        public TrainingParticipationAppService(IRepository<TrainingParticipation, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.TrainingParticipation_Read;
            GetAllPermissionName = PermissionNames.TrainingParticipation_Read;
            CreatePermissionName = PermissionNames.TrainingParticipation_Create;
            UpdatePermissionName = PermissionNames.TrainingParticipation_Update;
            DeletePermissionName = PermissionNames.TrainingParticipation_Delete;
        }

        protected override IQueryable<TrainingParticipation> CreateFilteredQuery(PagedTrainingParticipationResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Notes != null && x.Notes.Contains(input.Keyword)))
                .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes != null && x.Notes.Contains(input.Notes))
                .WhereIf(input.Attended.HasValue, x => x.Attended == input.Attended.Value)
                .WhereIf(input.CompletionDate.HasValue, x => x.CompletionDate == input.CompletionDate.Value)
                .WhereIf(input.Score.HasValue, x => x.Score == input.Score.Value)
                .WhereIf(input.CertificateEarned.HasValue, x => x.CertificateEarned == input.CertificateEarned.Value)
                .WhereIf(input.TrainingId.HasValue, x => x.TrainingId == input.TrainingId.Value)
                .WhereIf(input.EmployeeId.HasValue, x => x.EmployeeId == input.EmployeeId.Value);
        }

        public override async Task<TrainingParticipationDto> CreateAsync(CreateTrainingParticipationDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "TrainingParticipation", result);
            return result;
        }

        public override async Task<TrainingParticipationDto> UpdateAsync(TrainingParticipationDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "TrainingParticipation", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "TrainingParticipation", new { Id = input.Id });
        }
    }
}
