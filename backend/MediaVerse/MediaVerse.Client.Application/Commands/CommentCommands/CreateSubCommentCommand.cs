using MediatR;
using MediaVerse.Client.Application.DTOs.CommentDtos;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.CommentCommands;

public record CreateSubCommentCommand : IRequest<BaseResponse<GetCommentResponse>>
{
    public Guid ParentCommentId { get; set; }
    public PostCommentDto CommentDto { get; set; } = null!;
}



public class CreateSubCommentCommandHandler(
    IRepository<Comment> commentRepository,
    IRepository<Entry> entryRepository,
    IUserService userService)
    : IRequestHandler<CreateSubCommentCommand, BaseResponse<GetCommentResponse>>
{
    private readonly IRepository<Entry> _entryRepository = entryRepository;


    public async Task<BaseResponse<GetCommentResponse>> Handle(CreateSubCommentCommand request, CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetCommentResponse>(userResp.Exception);
        }

        var user = userResp.Data!;

        var parentComment = await commentRepository.GetByIdAsync(request.ParentCommentId, cancellationToken);
        if (parentComment is null)
        {
            return new BaseResponse<GetCommentResponse>(new NotFoundException());
        }

        var newComment = new Comment()
        {
            Id = Guid.NewGuid(),
            ParentCommentId = parentComment.Id,
            EntryId = parentComment.EntryId,
            UserId = user.Id,
            Content = request.CommentDto.Content,
        };
        newComment = await commentRepository.AddAsync(newComment, cancellationToken);

        var commentResponse = new GetCommentResponse()
        {
            Id = newComment.Id,
            EntryId = newComment.EntryId,
            Username = user.Username,
            UserProfile = user.ProfilePicture == null ? null :  Convert.ToBase64String( user.ProfilePicture.Picture),
            Content = newComment.Content,
            SubcommentCount = 0,
            Downvotes = 0,
            Upvotes = 0
        };
        return new BaseResponse<GetCommentResponse>(commentResponse);

    }
}