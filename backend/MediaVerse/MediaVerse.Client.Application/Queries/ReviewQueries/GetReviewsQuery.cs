using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.ReviewDtos;
using MediaVerse.Client.Application.Specifications.ReviewSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.ReviewQueries;

public record GetReviewsQuery(Guid EntryId) : IRequest<BaseResponse<GetReviewSummaryResponse>>;

public class GetReviewsQueryHandler(IRepository<Review> reviewRepository, IMapper mapper) : IRequestHandler<GetReviewsQuery, BaseResponse<GetReviewSummaryResponse>>
{
    public async Task<BaseResponse<GetReviewSummaryResponse>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var specification = new GetReviewsByEntryIdWithCritic(request.EntryId);
        var reviews = await reviewRepository.ListAsync(specification, cancellationToken);
        var reviewPreviews = mapper.Map<List<GetReviewPreviewResponse>>(reviews);
        var average = reviews.IsNullOrEmpty() ? 0m : reviews.Average(r => Convert.ToDecimal(r.Grade));
        var response = new GetReviewSummaryResponse()
        {
            GradeAvg = average,
            Reviews = reviewPreviews
        };
        return new BaseResponse<GetReviewSummaryResponse>(response);
    }
}