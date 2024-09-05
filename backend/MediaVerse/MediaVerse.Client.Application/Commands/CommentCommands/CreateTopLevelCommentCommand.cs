using MediatR;
using MediaVerse.Client.Application.Commands.Common;
using MediaVerse.Client.Application.DTOs.Comments;
using MediaVerse.Client.Application.Services.UserAccessor;
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
    CreateTopLevelCommentCommandHandler :IRequestHandler<CreateTopLevelCommentCommand,
    BaseResponse<GetCommentResponse>>
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<Entry> _entryRepository;
    private readonly IUserService _userService;


    public CreateTopLevelCommentCommandHandler(IRepository<Comment> commentRepository, IRepository<Entry> entryRepository, IUserService userService)
    {
        _commentRepository = commentRepository;
        _entryRepository = entryRepository;
        _userService = userService;
    }

    public async Task<BaseResponse<GetCommentResponse>> Handle(CreateTopLevelCommentCommand request,
        CancellationToken cancellationToken)
    {
        var userResp = await _userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetCommentResponse>(userResp.Exception);
        }
        var user = userResp.Data!;

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