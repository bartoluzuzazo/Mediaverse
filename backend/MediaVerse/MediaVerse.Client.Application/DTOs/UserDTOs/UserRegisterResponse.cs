namespace MediaVerse.Client.Application.DTOs.UserDTOs;

public class UserRegisterResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public Guid PictureId { get; set; }
}