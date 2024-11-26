using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetBookByIdSpecification : Specification<Book>
{
    public GetBookByIdSpecification(Guid id)
    {
        Query.Where(b => b.Id == id).Include(b => b.BookGenres);
    }
}