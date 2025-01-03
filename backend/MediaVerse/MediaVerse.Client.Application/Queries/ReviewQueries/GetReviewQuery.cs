using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.ReviewDtos;
using MediaVerse.Client.Application.Specifications.ReviewSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.ReviewQueries;

public record GetReviewQuery(Guid EntryId, Guid UserId) : IRequest<BaseResponse<GetReviewResponse>>;

public class GetReviewQueryHandler(IRepository<Review> reviewRepository, IMapper mapper) : IRequestHandler<GetReviewQuery, BaseResponse<GetReviewResponse>>
{
    public async Task<BaseResponse<GetReviewResponse>> Handle(GetReviewQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetReviewByIdsSpecification(request.UserId, request.EntryId);
        var review = await reviewRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (review is null)
        {
            return new BaseResponse<GetReviewResponse>(new NotFoundException("Review not found"));
        }

        var getReviewResponse = mapper.Map<GetReviewResponse>(review);
        return new BaseResponse<GetReviewResponse>(getReviewResponse);
    }
}