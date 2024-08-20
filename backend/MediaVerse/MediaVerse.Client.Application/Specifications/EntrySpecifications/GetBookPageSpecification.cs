using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public sealed class GetBookPageSpecification : Specification<Entry>
{
    public GetBookPageSpecification(int page, int size, EntryOrder order, OrderDirection direction)
    {
        Query.Include(e => e.Book).ThenInclude(b => b!.BookGenres)
            .Include(e => e.CoverPhoto)
            .Include(e => e.Ratings)
            .Include(e => e.WorkOns).ThenInclude(w => w.Author)
            .Include(e => e.WorkOns).ThenInclude(w => w.AuthorRole)
            .OrderEntry(order, direction).Skip((page - 1) * size).Take(size);
    }
}