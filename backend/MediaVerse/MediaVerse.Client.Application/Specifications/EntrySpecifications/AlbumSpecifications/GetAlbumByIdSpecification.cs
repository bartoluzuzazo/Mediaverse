using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;

public class GetAlbumByIdSpecification : Specification<Album>
{
    public GetAlbumByIdSpecification(Guid id)
    {
        Query.Where(a => a.Id == id).Include(a => a.MusicGenres);
    }
}