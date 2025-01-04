using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs.EpisodeDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs.SeriesDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.SeriesSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.EntryQueries.SeriesQueries;

public record GetSeriesQuery(Guid Id) : IRequest<BaseResponse<GetSeriesResponse>>;

public class GetSeriesQueryHandler(IRepository<Series> seriesRepository, IRepository<Entry> entryRepository)
    : GetBaseEntryQueryHandler(entryRepository), IRequestHandler<GetSeriesQuery, BaseResponse<GetSeriesResponse>>
{
    public async Task<BaseResponse<GetSeriesResponse>> Handle(GetSeriesQuery request, CancellationToken cancellationToken)
    {
        var query = new GetBaseEntryQuery(request.Id);
        var response = await base.Handle(query, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<GetSeriesResponse>(response.Exception);
        
        var spec = new GetSeriesByIdSpecification(request.Id);
        var series = await seriesRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (series is null) return new BaseResponse<GetSeriesResponse>(new NotFoundException());

        var seasons = series.Episodes.GroupBy(e => e.SeasonNumber)
            .Select(g => new GetSeriesSeasonResponse(g.Key, g.Select(e => {
                var ratingAvg = e.IdNavigation.Ratings.IsNullOrEmpty() ? 0m : e.IdNavigation.Ratings.Average(r => Convert.ToDecimal(r.Grade));
                return new EntryPreview
                {
                    Id = e.Id,
                    Name = e.IdNavigation.Name,
                    CoverPhoto = e.IdNavigation.CoverPhoto.Photo,
                    AvgRating = ratingAvg,
                    Description = e.IdNavigation.Description,
                    Type = e.IdNavigation.Type!,
                    ReleaseDate = e.IdNavigation.Release,
                    OrderNumber = e.EpisodeNumber
                };
            }).OrderBy(e => e.OrderNumber).ToList())).OrderBy(s => s.SeasonNumber).ToList();
        
        var responseBook = new GetSeriesResponse(series.CinematicGenres.Select(bg => bg.Name).ToList(),
            seasons, response.Data!);
        return new BaseResponse<GetSeriesResponse>(responseBook);
    }
}