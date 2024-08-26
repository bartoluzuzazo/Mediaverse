using System.Text;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.EntryCommands;

public record AddMovieCommand : IRequest<BaseResponse<AddEntryResponse>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Release { get; set; }
    public string CoverPhoto { get; set; }
    public string Synopsis { get; set; }
    public List<Guid> GenreIds { get; set; }
}

public class AddMovieCommandHandler(
    IRepository<Movie> movieRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<CinematicGenre> cinematicGenreRepository) : IRequestHandler<AddMovieCommand, BaseResponse<AddEntryResponse>>
{
    public async Task<BaseResponse<AddEntryResponse>> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        var photo = new CoverPhoto()
        {
            Id = Guid.NewGuid(),
            Photo = Encoding.ASCII.GetBytes(request.CoverPhoto),
        };
        
        var entry = new Entry()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Release = DateOnly.FromDateTime(request.Release),
            CoverPhotoId = photo.Id
        };
        
        var movie = new Movie()
        {
            Id = entry.Id,
            Synopsis = request.Synopsis,
            CinematicGenres = new List<CinematicGenre>()
        };
        
        var genres = request.GenreIds.Select(async id =>
        {
            var genre = await cinematicGenreRepository.GetByIdAsync(id, cancellationToken);
            return genre;
        }).ToList();
        
        foreach (var genre in genres)
        {
            var awaited = await genre;
            if (awaited is not null) movie.CinematicGenres.Add(awaited);
        }

        var response = new AddEntryResponse()
        {
            Id = entry.Id,
            CoverPhotoId = photo.Id
        };

        await photoRepository.AddAsync(photo, cancellationToken);
        await entryRepository.AddAsync(entry, cancellationToken);
        await movieRepository.AddAsync(movie, cancellationToken);
        
        return new BaseResponse<AddEntryResponse>(response);
    }
}
