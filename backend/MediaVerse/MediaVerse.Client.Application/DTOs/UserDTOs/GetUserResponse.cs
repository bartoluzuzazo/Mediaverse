namespace MediaVerse.Client.Application.DTOs.UserDTOs;

public class GetUserResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string ProfilePicture { get; set; } = null!;
}