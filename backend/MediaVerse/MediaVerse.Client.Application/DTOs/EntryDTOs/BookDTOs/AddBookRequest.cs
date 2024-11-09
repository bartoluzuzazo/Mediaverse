using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;

public record AddBookRequest(string Name, string Description, DateTime Release, string CoverPhoto,
    List<IEntryWorkOnRequest>? WorkOnRequests, string Isbn, string Synopsis, List<string>? Genres);