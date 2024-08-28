using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetBookGenresByIdSpecification : Specification<BookGenre>
{
    public GetBookGenresByIdSpecification(List<Guid> ids)
    {
        Query.Where(bg => ids.Contains(bg.Id));
    }
}