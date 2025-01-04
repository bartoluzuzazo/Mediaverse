using MediaVerse.Client.Application.DTOs.AuthorDTOs;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.AlbumDTOs;

public record GetAlbumResponse(List<string> MusicGenres, GetEntryResponse Entry, List<EntryPreview> Songs);
