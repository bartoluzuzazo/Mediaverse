using MediaVerse.Client.Application.DTOs.AuthorDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.EpisodeDTOs;

public record GetEpisodeResponse(EntryPreview Series, string Synopsis, int SeasonNumber, int EpisodeNumber, GetEntryResponse Entry);