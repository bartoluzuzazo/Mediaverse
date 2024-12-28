namespace MediaVerse.Client.Application.DTOs.EntryDTOs.EpisodeDTOs;

public record GetEpisodeResponse(Guid SeriesId, string SeriesName, string Synopsis, int SeasonNumber, int EpisodeNumber, GetEntryResponse Entry);