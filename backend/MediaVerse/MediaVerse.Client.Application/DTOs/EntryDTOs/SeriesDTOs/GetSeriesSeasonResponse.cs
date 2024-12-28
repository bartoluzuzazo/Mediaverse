using MediaVerse.Client.Application.DTOs.EntryDTOs.EpisodeDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.SeriesDTOs;

public record GetSeriesSeasonResponse(int SeasonNumber, List<GetSeriesEpisodeResponse> Episodes);