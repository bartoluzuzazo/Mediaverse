namespace MediaVerse.Client.Application.DTOs.EntryDTOs.EpisodeDTOs;

public record PatchEpisodeRequest(PatchEntryRequest Entry, int? SeasonNumber, int? EpisodeNumber, string? Synopsis, Guid? SeriesId);