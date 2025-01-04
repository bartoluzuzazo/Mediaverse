using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.GenresSpecifications;

public class GetMusicGenresByNameSpecification : Specification<MusicGenre>
{
    public GetMusicGenresByNameSpecification(List<string> names)
    {
        Query.Where(mg => names.Contains(mg.Name));
    }
}