using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;

public class GetBookResponse
{
    public GetEntryResponse Entry { get; set; }
    
    public string Isbn { get; set; } = null!;

    public string Synopsis { get; set; } = null!;
    
    public List<string> BookGenres { get; set; }
}