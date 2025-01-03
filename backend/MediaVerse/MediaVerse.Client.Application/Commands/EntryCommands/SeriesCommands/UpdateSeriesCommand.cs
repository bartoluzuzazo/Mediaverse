using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.SeriesDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.SeriesSpecifications;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.EpisodeSpecifications;
using MediaVerse.Client.Application.Specifications.GenresSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands.SeriesCommands;

public record UpdateSeriesCommand(Guid Id, PatchSeriesRequest Dto) : IRequest<BaseResponse<Guid>>;

public class UpdateSeriesCommandHandler(
    IRepository<Series> seriesRepository,
    IRepository<Episode> episodeRepository,
    IRepository<CinematicGenre> cinematicGenreRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> coverPhotoRepository,
    IRepository<AuthorRole> roleRepository,
    IRepository<WorkOn> workOnRepository,
    IMapper mapper)
    : UpdateEntryCommandHandler(entryRepository, coverPhotoRepository, roleRepository, workOnRepository, mapper),
        IRequestHandler<UpdateSeriesCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(UpdateSeriesCommand request, CancellationToken cancellationToken)
    {
        var command = new UpdateEntryCommand(request.Id, request.Dto.Entry);
        var response = await base.Handle(command, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<Guid>(response.Exception);
        
        var seriesSpecification = new GetSeriesByIdSpecification(request.Id);
        var series = await seriesRepository.FirstOrDefaultAsync(seriesSpecification, cancellationToken);

        if (request.Dto.Genres is not null)
        {
            var genreSpec = new GetCinematicGenresByNameSpecification(request.Dto.Genres);
            var dbGenres = await cinematicGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Dto.Genres.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new CinematicGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await cinematicGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            series!.CinematicGenres = dbGenres;
        }

        if (request.Dto.EpisodeIds is not null)
        {
            var spec = new GetEpisodesByIdsSpecification(request.Dto.EpisodeIds!);
            var episodes = await episodeRepository.ListAsync(spec, cancellationToken);
            series!.Episodes = episodes;
        }
        
        await seriesRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(series!.Id);
    }
}