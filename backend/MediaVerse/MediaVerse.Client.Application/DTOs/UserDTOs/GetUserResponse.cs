using MediaVerse.Client.Application.DTOs.AuthorDTOs;

namespace MediaVerse.Client.Application.DTOs.UserDTOs;

public class GetUserResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string ProfilePicture { get; set; } = null!;
    public List<GetEntryAuthorResponse>? Authors { get; set; }
}