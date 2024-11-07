using MediaVerse.Client.Application.Commands.EntryCommands;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;

public record AddBookRequest(AddEntryCommand Entry, string Isbn, string Synopsis, List<string>? Genres);