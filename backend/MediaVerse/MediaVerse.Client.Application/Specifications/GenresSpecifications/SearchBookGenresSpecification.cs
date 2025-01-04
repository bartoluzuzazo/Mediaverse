using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.GenresSpecifications;

public class SearchBookGenresSpecification : Specification<BookGenre>
{
    public SearchBookGenresSpecification(string searchTerm, int pageNumber, int pageSize)
    {
        Query.Search(genre => genre.Name.ToLower(), "%" + searchTerm.ToLower() + "%")
            .Paginate(pageNumber, pageSize)
            .OrderBy(genre => genre.Name);
    }
}