using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetBookByIdSpecification : Specification<Entry>
{
    public GetBookByIdSpecification(Guid id)
    {
        Query.Where(e => e.Id == id)
            .Include(e => e.Book).ThenInclude(b => b.BookGenres)
            .Include(e => e.CoverPhoto)
            .Include(e => e.Ratings)
            .Include(e => e.WorkOns).ThenInclude(w => w.Author)
            .Include(e => e.WorkOns).ThenInclude(w => w.AuthorRole);
    }
}