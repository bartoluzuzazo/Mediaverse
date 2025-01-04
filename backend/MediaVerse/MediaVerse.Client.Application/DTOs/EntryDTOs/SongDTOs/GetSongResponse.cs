using MediaVerse.Client.Application.DTOs.AuthorDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.SongDTOs;

public record GetSongResponse(string Lyrics, List<string> MusicGenres, GetEntryResponse Entry, List<EntryPreview> Albums);
