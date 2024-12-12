using MediaVerse.Client.Application.DTOs.EntryDTOs.SeriesDTOs.EpisodeDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.SeriesDTOs;

public record GetSeriesResponse(List<string> CinematicGenres, List<GetSeriesSeasonResponse> Seasons, GetEntryResponse Entry);
