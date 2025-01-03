using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.SeriesSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.EntryQueries.SeriesQueries;

public record SearchSeriesQuery(int Page, int Size, string Query): IRequest<BaseResponse<Page<GetEntrySearchResponse>>>;

public class SearchSeriesQueryHandler(IMapper mapper,IRepository<Series> seriesRepository) : IRequestHandler<SearchSeriesQuery, BaseResponse<Page<GetEntrySearchResponse>>>
{
    public async Task<BaseResponse<Page<GetEntrySearchResponse>>> Handle(SearchSeriesQuery request, CancellationToken cancellationToken)
    {
        var spec = new SearchSeriesSpecification(request.Query,request.Page, request.Size);
        var series  = await seriesRepository.ListAsync(spec, cancellationToken);
        var seriesCount = await seriesRepository.CountAsync(spec, cancellationToken);
        var response = series.Select(s => new GetEntrySearchResponse(s.Id, s.IdNavigation.Name, Convert.ToBase64String(s.IdNavigation.CoverPhoto.Photo))).ToList();
        
        var page = new Page<GetEntrySearchResponse>(response, request.Page, seriesCount,request.Size);
        return new BaseResponse<Page<GetEntrySearchResponse>>(page);
    }
}