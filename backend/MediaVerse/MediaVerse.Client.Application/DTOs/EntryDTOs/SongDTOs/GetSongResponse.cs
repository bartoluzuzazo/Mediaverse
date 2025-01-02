namespace MediaVerse.Client.Application.DTOs.EntryDTOs.SongDTOs;

public record GetSongResponse(string Lyrics, List<string> MusicGenres, GetEntryResponse Entry);
