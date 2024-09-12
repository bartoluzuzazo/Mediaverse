using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.RatingDTOs;
using MediaVerse.Client.Application.DTOs.RatingDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.RatingSpecifications;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediaVerse.Client.Application.Commands.RatingCommands;

public record UpdateRatingCommand(Guid Id, PutRatingDto RatingDto) : IRequest<BaseResponse<GetRatingResponse>>;
    
public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, BaseResponse<GetRatingResponse>>
{
    private readonly IRepository<Rating> _ratingRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Entry> _entryRepository;
    private readonly IUserAccessor _userAccessor;

    public UpdateRatingCommandHandler(IRepository<Rating> ratingRepository, IUserAccessor userAccessor,
        IRepository<User> userRepository, IRepository<Entry> entryRepository)
    {
        _ratingRepository = ratingRepository;
        _userAccessor = userAccessor;
        _userRepository = userRepository;
        _entryRepository = entryRepository;
    }

    public async Task<BaseResponse<GetRatingResponse>> Handle(UpdateRatingCommand request,
        CancellationToken cancellationToken)
    {
        var userEmail = _userAccessor.Email;
        if (userEmail == null)
        {
            return new BaseResponse<GetRatingResponse>(new ProblemException());
        }

        var specification = new GetUserSpecification(userEmail);
        var user = await _userRepository.FirstOrDefaultAsync(specification, cancellationToken);


        if (user is null)
        {
            return new BaseResponse<GetRatingResponse>(new NotFoundException());
        }


        var rating = await _ratingRepository.GetByIdAsync(request.Id, cancellationToken);
        if (rating is null)
        {
            return new BaseResponse<GetRatingResponse>(new NotFoundException());
        }

        if (rating.UserId != user.Id)
        {
            return new BaseResponse<GetRatingResponse>(new ForbiddenException());
        }

        if (rating.EntryId != request.RatingDto.EntryId)
        {
            var byRelatedIdsSpec = new GetRatingByIdsSpecification(user.Id, request.RatingDto.EntryId);
            var conflictingRating = await _ratingRepository.FirstOrDefaultAsync(byRelatedIdsSpec);
            if (conflictingRating is not null)
            {
                return new BaseResponse<GetRatingResponse>(new ConflictException());
            }

            var entry = await _entryRepository.GetByIdAsync(request.RatingDto.EntryId, cancellationToken);
            if (entry is null)
            {
                return new BaseResponse<GetRatingResponse>(new NotFoundException());
            }

            rating.Entry = entry;
        }

        rating.User = user;
        rating.Grade = request.RatingDto.Grade;
        rating.Modifiedat = DateTime.Now;
        await _ratingRepository.SaveChangesAsync(cancellationToken);

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