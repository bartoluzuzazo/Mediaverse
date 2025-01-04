namespace MediaVerse.Client.Application.DTOs.EntryDTOs.SeriesDTOs;

public record PatchSeriesRequest(PatchEntryRequest Entry, List<string>? Genres, List<Guid>? EpisodeIds);