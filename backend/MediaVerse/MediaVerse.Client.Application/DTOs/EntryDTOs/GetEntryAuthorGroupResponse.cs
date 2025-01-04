using MediaVerse.Client.Application.DTOs.AuthorDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs;

public class GetEntryAuthorGroupResponse
{
    public List<GetEntryAuthorResponse> Authors { get; set; }
    public string Role { get; set; }
}