using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetGameByIdSpecification : Specification<Game>
{
    public GetGameByIdSpecification(Guid id)
    {
        Query.Where(b => b.Id == id).Include(b => b.GameGenres);
    }
}