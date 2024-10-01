using MediatR;
using MediaVerse.Client.Application.Commands.Common;
using MediaVerse.Client.Application.DTOs.CommentDtos;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.CommentCommands;

public record CreateTopLevelCommentCommand : IRequest<BaseResponse<GetCommentResponse>>
{
    public Guid EntryId { get; set; }
    public PostCommentDto CommentDto { get; set; } = null!;
}

public class
    CreateTopLevelCommentCommandHandler(
        IRepository<Comment> commentRepository,
        IRepository<Entry> entryRepository,
        IUserService userService)
    : IRequestHandler<CreateTopLevelCommentCommand,
        BaseResponse<GetCommentResponse>>
{
    public async Task<BaseResponse<GetCommentResponse>> Handle(CreateTopLevelCommentCommand request,
        CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetCommentResponse>(userResp.Exception);
        }
        var user = userResp.Data!;

        var entry = await entryRepository.GetByIdAsync(request.EntryId, cancellationToken);
        if (entry is null)
        {
            return new BaseResponse<GetCommentResponse>(new NotFoundException());
        }

        var comment = new Comment()
        {
            Id = Guid.NewGuid(),
            Entry = entry,
            User = user,
            Content = request.CommentDto.Content
        };
        comment = await commentRepository.AddAsync(comment, cancellationToken);
        var commentResponse = new GetCommentResponse()
        {
            Id = comment.Id,
            EntryId = comment.EntryId,
            Username = user.Username,
            UserProfile = user.ProfilePicture == null ? null : Convert.ToBase64String(user.ProfilePicture.Picture),
            Content = comment.Content,
            SubcommentCount = 0,
            Upvotes = 0,
            Downvotes = 0
        };

        return new BaseResponse<GetCommentResponse>(commentResponse);
    }
}