namespace MediaVerse.Client.Application.DTOs.EntryDTOs.SongDTOs;

public class PatchSongRequest
{
    public PatchEntryRequest Entry { get; set; }
    public string? Lyrics { get; set; }
    public List<string>? Genres { get; set; }
}