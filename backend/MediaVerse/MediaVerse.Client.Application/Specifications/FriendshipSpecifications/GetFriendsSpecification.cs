using Ardalis.Specification;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.FriendshipSpecifications;

public class GetFriendsSpecification : Specification<Friendship, GetUserResponse>
{
    public GetFriendsSpecification(Guid userId)
    {
        Query.Where(f => (f.UserId == userId || f.User2Id == userId) && f.Approved);
        Query.Select(f => f.UserId == userId
            ? new GetUserResponse()
            {
                Id = f.User2.Id,
                ProfilePicture = Convert.ToBase64String(f.User2.ProfilePicture.Picture),
                Username = f.User2.Username
            }
            : new GetUserResponse()
            {
                Id = f.User.Id,
                ProfilePicture = Convert.ToBase64String(f.User.ProfilePicture.Picture),
                Username = f.User.Username
            });
    }
}