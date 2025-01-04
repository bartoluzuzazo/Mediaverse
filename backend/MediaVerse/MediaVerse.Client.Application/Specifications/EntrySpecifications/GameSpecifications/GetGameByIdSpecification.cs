using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.GameSpecifications;

public class GetGameByIdSpecification : Specification<Game>
{
    public GetGameByIdSpecification(Guid id)
    {
        Query.Where(g => g.Id == id).Include(g => g.GameGenres);
    }
}