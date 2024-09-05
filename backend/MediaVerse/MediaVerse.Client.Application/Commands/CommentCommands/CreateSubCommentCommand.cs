using MediatR;
using MediaVerse.Client.Application.Commands.Common;
using MediaVerse.Client.Application.DTOs.Comments;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.CommentCommands;

public record CreateSubCommentCommand : IRequest<BaseResponse<GetCommentResponse>>
{
    public Guid ParentCommentId { get; set; }
    public string Content { get; set; } = null!;
}



public class CreateSubCommentCommandHandler :UserAccessHandler, IRequestHandler<CreateSubCommentCommand, BaseResponse<GetCommentResponse>>
{

    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<Entry> _entryRepository;


    public CreateSubCommentCommandHandler(IUserAccessor userAccessor, IRepository<User> userRepository, IRepository<Comment> commentRepository, IRepository<Entry> entryRepository) : base(userAccessor, userRepository)
    {
        _commentRepository = commentRepository;
        _entryRepository = entryRepository;
    }

    public async Task<BaseResponse<GetCommentResponse>> Handle(CreateSubCommentCommand request, CancellationToken cancellationToken)
    {
        var userResp = await GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetCommentResponse>(userResp.Exception);
        }

        var user = userResp.Data!;

        var parentComment = await _commentRepository.GetByIdAsync(request.ParentCommentId, cancellationToken);
        if (parentComment is null)
        {
            return new BaseResponse<GetCommentResponse>(new NotFoundException());
        }

        var newComment = new Comment()
        {
            Id = Guid.NewGuid(),
            EntryId = parentComment.EntryId,
            UserId = user.Id,
            Content = request.Content,
        };
        newComment = await _commentRepository.AddAsync(newComment, cancellationToken);

        var commentResponse = new GetCommentResponse()
        {
            Id = newComment.Id,
            EntryId = newComment.EntryId,
            Username = user.Username,
            UserProfile =user.ProfilePicture == null ? null :  Convert.ToBase64String( user.ProfilePicture.Picture),
            Content = newComment.Content,
            SubcommentCount = 0,
            VoteSum = 0
        };
        return new BaseResponse<GetCommentResponse>(commentResponse);

    }
}