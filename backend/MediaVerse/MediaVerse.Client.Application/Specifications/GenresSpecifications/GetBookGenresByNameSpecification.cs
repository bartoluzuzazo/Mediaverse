using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.GenresSpecifications;

public class GetBookGenresByNameSpecification : Specification<BookGenre>
{
    public GetBookGenresByNameSpecification(List<string> names)
    {
        Query.Where(bg => names.Contains(bg.Name));
    }
}