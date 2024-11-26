using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs;

public class AddBaseEntryResponse
{
    public AddBaseEntryResponse(Entry entry, CoverPhoto photo)
    {
        Entry = entry;
        Photo = photo;
    }

    public Entry Entry { get; set; }
    public CoverPhoto Photo { get; set; }
}