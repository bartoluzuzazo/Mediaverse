using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;

public class GetAlbumByIdSpecification : Specification<Album>
{
    public GetAlbumByIdSpecification(Guid id)
    {
        Query.Where(a => a.Id == id).Include(a => a.MusicGenres)
            .Include(a => a.Songs).ThenInclude(s => s.IdNavigation).ThenInclude(e => e.Ratings)
            .Include(a => a.Songs).ThenInclude(s => s.IdNavigation).ThenInclude(e => e.CoverPhoto);
    }
}