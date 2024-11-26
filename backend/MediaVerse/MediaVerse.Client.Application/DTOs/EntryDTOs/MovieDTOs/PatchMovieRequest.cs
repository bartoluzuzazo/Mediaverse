namespace MediaVerse.Client.Application.DTOs.EntryDTOs.MovieDTOs;

public class PatchMovieRequest
{
    public PatchEntryRequest Entry { get; set; }
    public string? Synopsis { get; set; }
    public List<string>? Genres { get; set; }
}