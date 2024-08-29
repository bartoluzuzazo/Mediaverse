using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.RatingDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.RatingSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediaVerse.Client.Application.Commands.RatingCommands;

public record UpdateRatingCommand : IRequest<BaseResponse<GetRatingResponse>>
{
    public Guid Id { get; set; }

    public int Grade { get; set; }

    public Guid UserId { get; set; }

    public Guid EntryId { get; set; }

    public DateTime Modifiedat { get; set; }
}

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
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        var userEmail = _userAccessor.Email;
        if (userEmail == null)
        {
            return new BaseResponse<GetRatingResponse>(new ProblemException());
        }

        if (user is null || user.Email != userEmail)
        {
            return new BaseResponse<GetRatingResponse>(new ForbiddenException());
        }


        var rating = await _ratingRepository.GetByIdAsync(request.Id, cancellationToken);
        if (rating is null)
        {
            return new BaseResponse<GetRatingResponse>(new NotFoundException());
        }

        if (rating.EntryId != request.EntryId || rating.UserId != request.UserId)
        {
            var byRelatedIdsSpec = new GetRatingByIdsSpecification(request.UserId, request.EntryId);
            var conflictingRating = await _ratingRepository.FirstOrDefaultAsync(byRelatedIdsSpec);
            if (conflictingRating is not null)
            {
                return new BaseResponse<GetRatingResponse>(new ConflictException());
            }
            
        }

        if (rating.EntryId != request.EntryId)
        {
            var entry = await _entryRepository.GetByIdAsync(request.EntryId, cancellationToken);
            if (entry is null)
            {
                return new BaseResponse<GetRatingResponse>(new NotFoundException());
            }

            rating.Entry = entry;
        }

        rating.User = user;
        rating.Grade = request.Grade;
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