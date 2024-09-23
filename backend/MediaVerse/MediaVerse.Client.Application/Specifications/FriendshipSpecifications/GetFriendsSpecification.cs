using Ardalis.Specification;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.FriendshipSpecifications;

public class GetFriendsSpecification : Specification<Friendship, User>
{
    public GetFriendsSpecification(Guid userId)
    {
        Query.Where(f => (f.UserId == userId || f.User2Id == userId) && f.Approved);
        Query.Select(f => f.UserId == userId ? f.User2 : f.User);
    }
}