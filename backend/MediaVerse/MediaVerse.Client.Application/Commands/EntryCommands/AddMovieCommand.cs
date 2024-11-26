using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands;

public record AddMovieCommand(AddEntryCommand Entry, string Synopsis, List<string>? Genres) : IRequest<BaseResponse<Guid>>;

public class AddMovieCommandHandler(
    IRepository<Movie> movieRepository,
    IRepository<CinematicGenre> cinematicGenreRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<WorkOn> workOnRepository,
    IRepository<AuthorRole> roleRepository,
    IMapper mapper)
    : AddEntryCommandHandler(entryRepository, photoRepository, workOnRepository, roleRepository, mapper),
        IRequestHandler<AddMovieCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        var entryResponse = await base.Handle(request.Entry, cancellationToken);

        var movie = new Movie()
        {
            Id = entryResponse.Data.EntryId,
            Synopsis = request.Synopsis,
            CinematicGenres = new List<CinematicGenre>()
        };

        if (!request.Genres.IsNullOrEmpty())
        {
            var genreSpec = new GetCinematicGenresByNameSpecification(request.Genres!);
            var dbGenres = await cinematicGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Genres!.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new CinematicGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await cinematicGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            movie.CinematicGenres = dbGenres;
        }

        await movieRepository.AddAsync(movie, cancellationToken);

        return new BaseResponse<Guid>(movie.Id);
    }
}