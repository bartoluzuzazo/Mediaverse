using MediaVerse.Client.Application.Commands.EntryCommands;

namespace MediaVerse.Client.Application.DTOs.EntryDTOs.MovieDTOs;

public record AddMovieRequest(AddEntryCommand Entry, string Synopsis, List<string>? Genres);