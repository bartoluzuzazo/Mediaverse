using MediaVerse.Client.Application.DTOs.WorkOnDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;

public class PatchBookRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? Release { get; set; }
    public string? CoverPhoto { get; set; }
    public string? Isbn { get; set; }
    public string? Synopsis { get; set; }
    public List<string>? Genres { get; set; }
    public List<EntryWorkOnRequest>? WorkOnRequests { get; set; }
}