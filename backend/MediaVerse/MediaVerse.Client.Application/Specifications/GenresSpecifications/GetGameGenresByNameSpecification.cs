using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.GenresSpecifications;

public class GetGameGenresByNameSpecification : Specification<GameGenre>
{
    public GetGameGenresByNameSpecification(List<string> names)
    {
        Query.Where(bg => names.Contains(bg.Name));
    }
}