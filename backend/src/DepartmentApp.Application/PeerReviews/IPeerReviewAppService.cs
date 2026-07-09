using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.PeerReviews.Dto;

namespace DepartmentApp.PeerReviews
{
    public interface IPeerReviewAppService : IAsyncCrudAppService<
        PeerReviewDto,
        long,
        PagedPeerReviewResultRequestDto,
        CreatePeerReviewDto,
        PeerReviewDto>
    {
    }
}
