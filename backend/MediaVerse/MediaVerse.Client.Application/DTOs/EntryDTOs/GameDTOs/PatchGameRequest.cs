namespace MediaVerse.Client.Application.DTOs.EntryDTOs.GameDTOs;

public class PatchGameRequest
{
    public PatchEntryRequest Entry { get; set; }
    public string? Synopsis { get; set; }
    public List<string>? Genres { get; set; }
}