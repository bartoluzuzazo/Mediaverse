using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs.EpisodeDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.SeriesDTOs;

public record GetSeriesSeasonResponse(int SeasonNumber, List<EntryPreview> Episodes);