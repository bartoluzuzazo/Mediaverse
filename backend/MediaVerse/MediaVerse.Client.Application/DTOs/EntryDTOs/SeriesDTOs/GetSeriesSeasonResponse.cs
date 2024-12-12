using MediaVerse.Client.Application.DTOs.EntryDTOs.SeriesDTOs.EpisodeDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.SeriesDTOs;

public record GetSeriesSeasonResponse(int SeasonNumber, List<GetSeriesEpisodeResponse> Episodes);