using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;

public class GetAlbumsByIdsSpecification : Specification<Album>
{
    public GetAlbumsByIdsSpecification(List<Guid> ids)
    {
        Query.Where(a => ids.Contains(a.Id));
    }
}