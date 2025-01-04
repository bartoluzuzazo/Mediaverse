using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.SongSpecifications;

public class SearchSongSpecification : Specification<Song>
{
    public SearchSongSpecification(string searchTerm, int pageNumber, int pageSize)
    {
        Query.Include(song => song.IdNavigation).ThenInclude(e => e.CoverPhoto)
            .Search(song => song.IdNavigation.Name.ToLower(), "%" + searchTerm.ToLower() + "%")
            .Paginate(pageNumber, pageSize)
            .OrderBy(song => song.IdNavigation.Name);
    }
}