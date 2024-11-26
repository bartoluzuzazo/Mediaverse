using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public sealed class GetBookPageSpecification : Specification<Entry>
{
    public GetBookPageSpecification(int page, int size, EntryOrder order, OrderDirection direction)
    {
        Query.Where(e => e.Book != null)
            .Include(e => e.Book).ThenInclude(b => b!.BookGenres)
            .IncludeEntry().OrderEntry(order, direction).Paginate(page, size);
    }
}