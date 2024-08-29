using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs.RatingDTOs;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.RatingSpecifications;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.RatingQueries;

public record GetUsersRatingQuery(Guid EntryGuid) : IRequest<BaseResponse<GetRatingResponse>>;

public class GetUsersRatingHandler(IRepository<Rating> ratingRepository, IUserAccessor userAccessor)
    : IRequestHandler<GetUsersRatingQuery, BaseResponse<GetRatingResponse>>
{
    public async Task<BaseResponse<GetRatingResponse>> Handle(GetUsersRatingQuery request, CancellationToken cancellationToken)
    {
        var email = userAccessor.Email;
        if (email is null)
        {
            return new BaseResponse<GetRatingResponse>(new ProblemException());
        }

        var spec = new GetRatingByUserNameSpecification(email);
        var rating = await ratingRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (rating == null)
        {
            return new BaseResponse<GetRatingResponse>(new NotFoundException());
        }

        var ratingResponse = new GetRatingResponse()
        {
            EntryId = rating.EntryId,
            Grade = rating.Grade,
            Id = rating.Id,
            UserId = rating.UserId
        };
        return new BaseResponse<GetRatingResponse>(ratingResponse);
    }
}