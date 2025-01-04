using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs.AlbumDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.EntryQueries.AlbumQueries;

public record GetAlbumQuery(Guid Id) : IRequest<BaseResponse<GetAlbumResponse>>;

public class GetAlbumQueryHandler(IRepository<Album> albumRepository, IRepository<Entry> entryRepository)
    : GetBaseEntryQueryHandler(entryRepository), IRequestHandler<GetAlbumQuery, BaseResponse<GetAlbumResponse>>
{
    public async Task<BaseResponse<GetAlbumResponse>> Handle(GetAlbumQuery request, CancellationToken cancellationToken)
    {
        var query = new GetBaseEntryQuery(request.Id);
        var response = await base.Handle(query, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<GetAlbumResponse>(response.Exception);
        
        var spec = new GetAlbumByIdSpecification(request.Id);
        var album = await albumRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (album is null) return new BaseResponse<GetAlbumResponse>(new NotFoundException());

        var songs = album.Songs.Select(s =>
        {
            var score = s.IdNavigation.Ratings.IsNullOrEmpty() ? 0m : s.IdNavigation.Ratings.Average(r => Convert.ToDecimal(r.Grade));
                        
            return new EntryPreview
            {
                Id = s.Id,
                Name = s.IdNavigation.Name,
                CoverPhoto = s.IdNavigation.CoverPhoto.Photo,
                AvgRating = score,
                Description = s.IdNavigation.Description,
                Type = s.IdNavigation.Type!,
                ReleaseDate = s.IdNavigation.Release
            };
        }).ToList();
        
        var responseAlbum = new GetAlbumResponse(album.MusicGenres.Select(bg => bg.Name).ToList(), response.Data, songs);
        return new BaseResponse<GetAlbumResponse>(responseAlbum);
    }
}