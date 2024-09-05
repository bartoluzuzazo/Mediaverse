using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.RatingDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.RatingSpecifications;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace MediaVerse.Client.Application.Commands.RatingCommands;

public record CreateRatingCommand : IRequest<BaseResponse<GetRatingResponse>>
{
    public int Grade { get; set; }
    public Guid EntryId { get; set; }

}

public class CreateRatingCommandHandler(
    IRepository<Rating> ratingRepository,
    IUserAccessor userAccessor,
    IRepository<User> userRepository,
    IRepository<Entry> entryRepository)
    : IRequestHandler<CreateRatingCommand, BaseResponse<GetRatingResponse>>
{
    public async Task<BaseResponse<GetRatingResponse>> Handle(CreateRatingCommand request,
        CancellationToken cancellationToken)
    {
        var email = userAccessor.Email;
        if (email == null)
        {
            return new BaseResponse<GetRatingResponse>(new ProblemException());
        }

        var userSpecification = new GetUserSpecification(email);
        var user = await userRepository.FirstOrDefaultAsync(userSpecification, cancellationToken);
        if (user is null)
        {
            return new BaseResponse<GetRatingResponse>(new NotFoundException());
        }

        var entry = await entryRepository.GetByIdAsync(request.EntryId, cancellationToken);
        if (entry is null)
        {
            return new BaseResponse<GetRatingResponse>(new NotFoundException());
        }

        var ratingSpecification = new GetRatingByIdsSpecification(user.Id, request.EntryId);

        var ratingFromDb = await ratingRepository.FirstOrDefaultAsync(ratingSpecification, cancellationToken);
        if (ratingFromDb is not null)
        {
            return new BaseResponse<GetRatingResponse>(new ConflictException());
        }

        var rating = new Rating()
        {
            Id = Guid.NewGuid(),
            Entry = entry,
            User = user,
            Modifiedat = DateTime.Now,
            Grade = request.Grade,
        };
        await ratingRepository.AddAsync(rating, cancellationToken);

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