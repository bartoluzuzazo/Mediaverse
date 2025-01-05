using MediaVerse.Client.Application.DTOs.WorkOnDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs;

public class PatchEntryRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? Release { get; set; }
    public string? Photo { get; set; }
    public List<EntryWorkOnRequest>? WorkOnRequests { get; set; }
}