namespace MediaVerse.Client.Application.DTOs.EntryDTOs.MovieDTOs;

public record GetMovieResponse(string Synopsis, List<string> CinematicGenres)
{
    public GetEntryResponse Entry { get; set; }
}