using MediaVerse.Client.Application.DTOs.AuthorDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs;

public class GetEntryResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly Release { get; set; }
    
    public string Photo { get; set; } = null!;

    public decimal RatingAvg { get; set; }

    public List<GetEntryAuthorGroupResponse> Authors { get; set; }
}