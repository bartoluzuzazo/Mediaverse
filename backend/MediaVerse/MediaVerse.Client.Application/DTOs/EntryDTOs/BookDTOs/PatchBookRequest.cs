namespace MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;

public class PatchBookRequest
{
    public PatchEntryRequest Entry { get; set; }
    public string? Isbn { get; set; }
    public string? Synopsis { get; set; }
    public List<string>? Genres { get; set; }
}