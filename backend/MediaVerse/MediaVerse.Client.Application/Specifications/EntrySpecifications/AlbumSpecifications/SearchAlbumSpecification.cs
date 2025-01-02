using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;

public class SearchAlbumSpecification : Specification<Album>
{
    public SearchAlbumSpecification(string searchTerm, int pageNumber, int pageSize)
    {
        Query.Include(album => album.IdNavigation).ThenInclude(e => e.CoverPhoto)
            .Search(album => album.IdNavigation.Name.ToLower(), "%" + searchTerm.ToLower() + "%")
            .Paginate(pageNumber, pageSize)
            .OrderBy(album => album.IdNavigation.Name);
    }
}