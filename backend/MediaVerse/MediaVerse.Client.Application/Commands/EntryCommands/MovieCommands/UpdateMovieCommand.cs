using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.MovieDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.MovieSpecifications;
using MediaVerse.Client.Application.Specifications.GenresSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.EntryCommands.MovieCommands;

public record UpdateMovieCommand(Guid Id, PatchMovieRequest Dto) : IRequest<BaseResponse<Guid>>;

public class UpdateMovieCommandHandler(
    IRepository<Movie> movieRepository,
    IRepository<CinematicGenre> cinematicGenreRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> coverPhotoRepository,
    IRepository<AuthorRole> roleRepository,
    IRepository<WorkOn> workOnRepository,
    IMapper mapper)
    : UpdateEntryCommandHandler(entryRepository, coverPhotoRepository, roleRepository, workOnRepository, mapper),
        IRequestHandler<UpdateMovieCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var command = new UpdateEntryCommand(request.Id, request.Dto.Entry);
        var response = await base.Handle(command, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<Guid>(response.Exception);
        
        var movieSpecification = new GetMovieByIdSpecification(request.Id);
        var movie = await movieRepository.FirstOrDefaultAsync(movieSpecification, cancellationToken);

        movie.Synopsis = request.Dto.Synopsis ?? movie.Synopsis;

        if (request.Dto.Genres is not null)
        {
            var genreSpec = new GetCinematicGenresByNameSpecification(request.Dto.Genres);
            var dbGenres = await cinematicGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Dto.Genres.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new CinematicGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await cinematicGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            movie.CinematicGenres = dbGenres;
        }

        await movieRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(movie.Id);
    }
}