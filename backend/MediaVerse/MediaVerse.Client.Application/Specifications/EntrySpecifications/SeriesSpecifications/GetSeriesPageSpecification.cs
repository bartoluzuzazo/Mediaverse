using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.SeriesSpecifications;

public class GetSeriesPageSpecification : Specification<Entry>
{
    public GetSeriesPageSpecification(int page, int size, EntryOrder order, OrderDirection direction)
    {
        Query.Where(e => e.Series != null)
            .Include(e => e.Series).ThenInclude(m => m!.CinematicGenres)
            .IncludeEntry().OrderEntry(order, direction).Paginate(page, size);
    }
}