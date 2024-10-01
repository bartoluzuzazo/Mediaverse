using MediatR;
using MediaVerse.Client.Application.Queries.FriendshipQueries;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.FriendshipSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.FriendshipCommands;

public record DeleteFriendshipCommand(Guid FriendId): IRequest<Exception?>;

public class DeleteFriendshipCommandHandler(IRepository<Friendship> friendshipRepository, IUserService userService): IRequestHandler<DeleteFriendshipCommand,Exception?>
{
    public async Task<Exception?> Handle(DeleteFriendshipCommand request, CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return userResp.Exception;
        }

        var user = userResp.Data!;
        var spec = new GetFriendshipSpecification(user.Id, request.FriendId);
        var friendship = await friendshipRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (friendship is not null)
        {
            await friendshipRepository.DeleteAsync(friendship, cancellationToken);
        }

        return null;
    }
}