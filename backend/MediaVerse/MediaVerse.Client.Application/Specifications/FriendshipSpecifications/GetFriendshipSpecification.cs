using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.FriendshipSpecifications;

public class GetFriendshipSpecification : Specification<Friendship>
{
    public GetFriendshipSpecification(Guid user1Id, Guid user2Id)
    {
        Query.Where(f =>
            (f.UserId == user1Id && f.User2Id == user2Id) || (f.UserId == user2Id && f.User2Id == user1Id));
    }
}