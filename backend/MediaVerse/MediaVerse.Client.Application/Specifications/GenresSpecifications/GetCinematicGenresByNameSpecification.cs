using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.GenresSpecifications;

public class GetCinematicGenresByNameSpecification : Specification<CinematicGenre>
{
    public GetCinematicGenresByNameSpecification(List<string> names)
    {
        Query.Where(cg => names.Contains(cg.Name));
    }
}