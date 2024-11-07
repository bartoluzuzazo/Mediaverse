using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetCinematicGenresByNameSpecification : Specification<CinematicGenre>
{
    public GetCinematicGenresByNameSpecification(List<string> names)
    {
        Query.Where(bg => names.Contains(bg.Name));
    }
}