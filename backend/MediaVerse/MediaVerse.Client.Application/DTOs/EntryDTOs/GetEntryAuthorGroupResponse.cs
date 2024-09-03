using MediaVerse.Client.Application.DTOs.AuthorDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs;

public class GetEntryAuthorGroupResponse
{
    public string Role { get; set; }
    public List<GetEntryAuthorResponse> Authors { get; set; }
}