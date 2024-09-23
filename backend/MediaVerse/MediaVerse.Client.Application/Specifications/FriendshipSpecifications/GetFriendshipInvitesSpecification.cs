using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.FriendshipSpecifications;

public class GetFriendshipInvitesSpecification : Specification<Friendship>
{
    public GetFriendshipInvitesSpecification(Guid userId)
    {
        Query.Where(friendship => friendship.User2Id == userId && !friendship.Approved)
            .Include(friendship => friendship.User);
    }
}