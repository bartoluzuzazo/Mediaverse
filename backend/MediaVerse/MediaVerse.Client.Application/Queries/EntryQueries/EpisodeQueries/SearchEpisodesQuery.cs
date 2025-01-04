using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.EpisodeSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.EntryQueries.EpisodeQueries;

public record SearchEpisodesQuery(int Page, int Size, string Query): IRequest<BaseResponse<Page<GetEntrySearchResponse>>>;

public class SearchEpisodesQueryHandler(IMapper mapper,IRepository<Episode> episodeRepository) : IRequestHandler<SearchEpisodesQuery, BaseResponse<Page<GetEntrySearchResponse>>>
{
    public async Task<BaseResponse<Page<GetEntrySearchResponse>>> Handle(SearchEpisodesQuery request, CancellationToken cancellationToken)
    {
        var spec = new SearchEpisodesSpecification(request.Query,request.Page, request.Size);
        var episodes  = await episodeRepository.ListAsync(spec, cancellationToken);
        var episodeCount = await episodeRepository.CountAsync(spec, cancellationToken);
        var response = episodes.Select(s => new GetEntrySearchResponse(s.Id, s.IdNavigation.Name, Convert.ToBase64String(s.IdNavigation.CoverPhoto.Photo))).ToList();
        
        var page = new Page<GetEntrySearchResponse>(response, request.Page, episodeCount,request.Size);
        return new BaseResponse<Page<GetEntrySearchResponse>>(page);
    }
}