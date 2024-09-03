using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Extensions.EntryExtensions;

public static class EntryExtensions
{
    public static string GetTypeOfEntry(this Entry entry)
    {
        if (entry.Book is not null) return "Book";
        if (entry.Album is not null) return "Album";
        if (entry.Song is not null) return "Song";
        if (entry.Game is not null) return "Game";
        if (entry.Movie is not null) return "Movie";
        if (entry.Series is not null) return "Series";
        if (entry.Episode is not null) return "Episode";
        return "Entry";
    }
}