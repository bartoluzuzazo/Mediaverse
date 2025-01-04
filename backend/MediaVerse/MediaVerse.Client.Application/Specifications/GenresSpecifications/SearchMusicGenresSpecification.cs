using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.GenresSpecifications;

public class SearchMusicGenresSpecification : Specification<MusicGenre>
{
    public SearchMusicGenresSpecification(string searchTerm, int pageNumber, int pageSize)
    {
        Query.Search(genre => genre.Name.ToLower(), "%" + searchTerm.ToLower() + "%")
            .Paginate(pageNumber, pageSize)
            .OrderBy(genre => genre.Name);
    }
}