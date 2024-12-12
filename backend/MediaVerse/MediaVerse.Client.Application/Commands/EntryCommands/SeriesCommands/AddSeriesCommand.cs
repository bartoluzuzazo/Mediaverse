using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.Specifications.GenresSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands.SeriesCommands;

public record AddSeriesCommand(AddEntryCommand Entry, string Synopsis, List<string>? Genres) : IRequest<BaseResponse<Guid>>;

public class AddSeriesCommandHandler(
    IRepository<Series> SeriesRepository,
    IRepository<CinematicGenre> cinematicGenreRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<WorkOn> workOnRepository,
    IRepository<AuthorRole> roleRepository,
    IMapper mapper)
    : AddEntryCommandHandler(entryRepository, photoRepository, workOnRepository, roleRepository, mapper),
        IRequestHandler<AddSeriesCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(AddSeriesCommand request, CancellationToken cancellationToken)
    {
        var entryResponse = await base.Handle(request.Entry, cancellationToken);

        var Series = new Series()
        {
            Id = entryResponse.Data.EntryId,
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
            Series.CinematicGenres = dbGenres;
        }

        await SeriesRepository.AddAsync(Series, cancellationToken);

        return new BaseResponse<Guid>(Series.Id);
    }
}