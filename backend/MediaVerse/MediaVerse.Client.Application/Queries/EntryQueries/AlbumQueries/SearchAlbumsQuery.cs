using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.EntryQueries.AlbumQueries;

public record SearchAlbumsQuery(int Page, int Size, string Query): IRequest<BaseResponse<Page<GetEntrySearchResponse>>>;

public class SearchAlbumsQueryHandler(IMapper mapper,IRepository<Album> albumRepository) : IRequestHandler<SearchAlbumsQuery, BaseResponse<Page<GetEntrySearchResponse>>>
{
    public async Task<BaseResponse<Page<GetEntrySearchResponse>>> Handle(SearchAlbumsQuery request, CancellationToken cancellationToken)
    {
        var spec = new SearchAlbumSpecification(request.Query,request.Page, request.Size);
        var albums  = await albumRepository.ListAsync(spec, cancellationToken);
        var albumCount = await albumRepository.CountAsync(spec, cancellationToken);
        var response = albums.Select(s => new GetEntrySearchResponse(s.Id, s.IdNavigation.Name, Convert.ToBase64String(s.IdNavigation.CoverPhoto.Photo))).ToList();
        
        var page = new Page<GetEntrySearchResponse>(response, request.Page, albumCount,request.Size);
        return new BaseResponse<Page<GetEntrySearchResponse>>(page);
    }
}