namespace MediaVerse.Client.Application.DTOs.EntryDTOs.AlbumDTOs;

public class PatchAlbumRequest
{
    public PatchEntryRequest Entry { get; set; }
    public List<string>? Genres { get; set; }
}