namespace MediaVerse.Client.Application.DTOs.EntryDTOs.EpisodeDTOs;

public record GetSeriesEpisodeResponse(Guid Id, string Name, int EpisodeNumber, DateOnly Release, decimal RatingAvg, string Synopsis);
