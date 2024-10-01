namespace MediaVerse.Client.Application.DTOs.FriendshipDTOs;

public class GetFriendshipResponse
{
    public Guid UserId { get; set; }

    public Guid User2Id { get; set; }

    public bool Approved { get; set; }

}