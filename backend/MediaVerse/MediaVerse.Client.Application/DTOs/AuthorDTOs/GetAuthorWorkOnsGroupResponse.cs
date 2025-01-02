namespace MediaVerse.Client.Application.DTOs.AuthorDTOs;

public class GetAuthorWorkOnsGroupResponse
{
    public string Role { get; set; }
    public List<EntryPreview> Entries { get; set; }
}