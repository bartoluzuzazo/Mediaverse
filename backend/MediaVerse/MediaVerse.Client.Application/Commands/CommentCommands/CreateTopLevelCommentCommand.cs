using MediatR;
using MediaVerse.Client.Application.DTOs.Comments;
using MediaVerse.Client.Application.DTOs.EntryDTOs.RatingDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.CommentCommands;

public record CreateTopLevelCommentCommand : IRequest<BaseResponse<GetCommentResponse>>
{
    public Guid EntryId { get; set; }
    public string Content { get; set; } = null!;
}

public class
    CreateTopLevelCommentCommandHandler : IRequestHandler<CreateTopLevelCommentCommand,
    BaseResponse<GetCommentResponse>>
{
    private readonly IUserAccessor _userAccessor;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<Entry> _entryRepository;

    public CreateTopLevelCommentCommandHandler(IRepository<Comment> commentRepository, IRepository<User> userRepository,
        IUserAccessor userAccessor, IRepository<Entry> entryRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _userAccessor = userAccessor;
        _entryRepository = entryRepository;
    }


    public async Task<BaseResponse<GetCommentResponse>> Handle(CreateTopLevelCommentCommand request,
        CancellationToken cancellationToken)
    {
        var userEmail = _userAccessor.Email;
        if (userEmail is null)
        {
            return new BaseResponse<GetCommentResponse>(new ProblemException());
        }

        var userSpec = new GetUserSpecification(userEmail);
        var user = await _userRepository.FirstOrDefaultAsync(userSpec, cancellationToken);
        if (user is null)
        {
            return new BaseResponse<GetCommentResponse>(new NotFoundException());
        }

        var entry = await _entryRepository.GetByIdAsync(request.EntryId, cancellationToken);
        if (entry is null)
        {
            return new BaseResponse<GetCommentResponse>(new NotFoundException());
        }

        var comment = new Comment()
        {
            Id = Guid.NewGuid(),
            Entry = entry,
            User = user,
            Content = request.Content
        };
        comment = await _commentRepository.AddAsync(comment, cancellationToken);
        var commentResponse = new GetCommentResponse()
        {
            Id = comment.Id,
            EntryId = comment.EntryId,
            Username = user.Username,
            UserProfile = user.ProfilePicture == null ? null : Convert.ToBase64String(user.ProfilePicture.Picture),
            Content = comment.Content,
            SubcommentCount = 0,
            VoteSum = 0
        };

        return new BaseResponse<GetCommentResponse>(commentResponse);
    }
}