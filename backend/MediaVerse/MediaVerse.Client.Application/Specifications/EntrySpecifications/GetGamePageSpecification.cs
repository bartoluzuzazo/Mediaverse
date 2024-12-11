using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetGamePageSpecification : Specification<Entry>
{
    public GetGamePageSpecification(int page, int size, EntryOrder order, OrderDirection direction)
    {
        Query.Where(e => e.Game != null)
            .Include(e => e.Game).ThenInclude(m => m!.GameGenres)
            .IncludeEntry().OrderEntry(order, direction).Paginate(page, size);
    }
}