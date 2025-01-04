namespace MediaVerse.Client.Application.DTOs.AuthorDTOs;

public class GetEntryAuthorResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }
    
    public string? Details { get; set; }

    public byte[] ProfilePicture { get; set; }
}