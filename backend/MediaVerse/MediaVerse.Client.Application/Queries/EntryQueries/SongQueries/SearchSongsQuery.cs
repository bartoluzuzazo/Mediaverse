using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.SongSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.EntryQueries.SongQueries;

public record SearchSongsQuery(int Page, int Size, string Query): IRequest<BaseResponse<Page<GetEntrySearchResponse>>>;

public class SearchSongsQueryHandler(IMapper mapper,IRepository<Song> songRepository) : IRequestHandler<SearchSongsQuery, BaseResponse<Page<GetEntrySearchResponse>>>
{
    public async Task<BaseResponse<Page<GetEntrySearchResponse>>> Handle(SearchSongsQuery request, CancellationToken cancellationToken)
    {
        var spec = new SearchSongSpecification(request.Query,request.Page, request.Size);
        var songs  = await songRepository.ListAsync(spec, cancellationToken);
        var songCount = await songRepository.CountAsync(spec, cancellationToken);
        var response = songs.Select(s => new GetEntrySearchResponse(s.Id, s.IdNavigation.Name, Convert.ToBase64String(s.IdNavigation.CoverPhoto.Photo))).ToList();
        
        var page = new Page<GetEntrySearchResponse>(response, request.Page, songCount,request.Size);
        return new BaseResponse<Page<GetEntrySearchResponse>>(page);
    }
}