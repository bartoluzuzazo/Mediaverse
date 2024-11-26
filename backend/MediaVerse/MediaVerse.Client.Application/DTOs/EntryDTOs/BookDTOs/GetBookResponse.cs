using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;

public record GetBookResponse(string Isbn, string Synopsis, List<string> BookGenres, GetEntryResponse Entry);