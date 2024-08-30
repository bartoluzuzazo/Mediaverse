using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.RatingDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.RatingSpecifications;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.RatingCommands;

public record CreateRatingCommand : IRequest<BaseResponse<GetRatingResponse>>
{
    public int Grade { get; set; }


    public Guid EntryId { get; set; }

    public DateTime Modifiedat { get; set; }
}

public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, BaseResponse<GetRatingResponse>>
{
    private readonly IRepository<Rating> _ratingRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Entry> _entryRepository;
    private readonly IUserAccessor _userAccessor;

    public CreateRatingCommandHandler(IRepository<Rating> ratingRepository, IUserAccessor userAccessor,
        IRepository<User> userRepository, IRepository<Entry> entryRepository)
    {
        _ratingRepository = ratingRepository;
        _userAccessor = userAccessor;
        _userRepository = userRepository;
        _entryRepository = entryRepository;
    }

    public async Task<BaseResponse<GetRatingResponse>> Handle(CreateRatingCommand request,
        CancellationToken cancellationToken)
    {
        var email = _userAccessor.Email;
        if (email == null)
        {
            return new BaseResponse<GetRatingResponse>(new ProblemException());
        }

        var userSpecification = new GetUserSpecification(email);
        var user = await _userRepository.FirstOrDefaultAsync(userSpecification);
        if (user is null)
        {
            return new BaseResponse<GetRatingResponse>(new NotFoundException());
        }

        var entry = await _entryRepository.GetByIdAsync(request.EntryId, cancellationToken);
        if (entry is null)
        {
            return new BaseResponse<GetRatingResponse>(new NotFoundException());
        }

        var ratingSpecification = new GetRatingByIdsSpecification(user.Id, request.EntryId);

        var ratingFromDb = await _ratingRepository.FirstOrDefaultAsync(ratingSpecification, cancellationToken);
        if (ratingFromDb is not null)
        {
            return new BaseResponse<GetRatingResponse>(new ConflictException());
        }

        var rating = new Rating()
        {
            Entry = entry,
            User = user,
            Modifiedat = DateTime.Now,
            Grade = request.Grade,
        };
        await _ratingRepository.AddAsync(rating, cancellationToken);

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