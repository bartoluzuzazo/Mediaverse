namespace MediaVerse.Client.Application.DTOs.EntryDTOs.GameDTOs;

public record GetGameResponse(string Synopsis, List<string> GameGenres, GetEntryResponse Entry);
