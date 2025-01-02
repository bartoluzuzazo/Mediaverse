using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public sealed class GetBookByIsbnSpecification : Specification<Book>
{
    public GetBookByIsbnSpecification(string isbn)
    {
        Query.Where(b => b.Isbn == isbn).Include(b => b.BookGenres);
    }
}