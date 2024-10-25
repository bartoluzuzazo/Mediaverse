using MediatR;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.CommentCommands;

public record DeleteCommentCommand(Guid CommentId) : IRequest<Exception?>;

public class DeleteCommentCommandHandler(IUserService userService, IRepository<Comment> commentRepository) : IRequestHandler<DeleteCommentCommand,Exception?>
{
    public async Task<Exception?> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return userResp.Exception;
        }

        var user = userResp.Data!;

        var comment = await commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment is null)
        {
            return new NotFoundException();
        }

        if (comment.UserId != user.Id)
        {
            return new ForbiddenException();
        }
        comment.DeletedAt = DateOnly.FromDateTime(DateTime.Now);
        await commentRepository.SaveChangesAsync(cancellationToken);
        return null;
    }
}