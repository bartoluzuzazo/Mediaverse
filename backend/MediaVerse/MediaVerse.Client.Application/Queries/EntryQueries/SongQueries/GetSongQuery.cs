using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs.SongDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.SongSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.EntryQueries.SongQueries;

public record GetSongQuery(Guid Id) : IRequest<BaseResponse<GetSongResponse>>;

public class GetSongQueryHandler(IRepository<Song> songRepository, IRepository<Entry> entryRepository)
    : GetBaseEntryQueryHandler(entryRepository), IRequestHandler<GetSongQuery, BaseResponse<GetSongResponse>>
{
    public async Task<BaseResponse<GetSongResponse>> Handle(GetSongQuery request, CancellationToken cancellationToken)
    {
        var query = new GetBaseEntryQuery(request.Id);
        var response = await base.Handle(query, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<GetSongResponse>(response.Exception);
        
        var spec = new GetSongByIdSpecification(request.Id);
        var song = await songRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (song is null) return new BaseResponse<GetSongResponse>(new NotFoundException());

        var albums = song.Albums.Select(al =>
        {
            var score = al.IdNavigation.Ratings.IsNullOrEmpty() ? 0m : al.IdNavigation.Ratings.Average(r => Convert.ToDecimal(r.Grade));
                        
            return new EntryPreview
            {
                Id = al.Id,
                Name = al.IdNavigation.Name,
                CoverPhoto = al.IdNavigation.CoverPhoto.Photo,
                AvgRating = score,
                Description = al.IdNavigation.Description,
                Type = al.IdNavigation.Type!,
                ReleaseDate = al.IdNavigation.Release
            };
        }).ToList();
        var responseSong = new GetSongResponse(song.Lyrics, song.MusicGenres.Select(bg => bg.Name).ToList(), response.Data, albums);
        return new BaseResponse<GetSongResponse>(responseSong);
    }
}