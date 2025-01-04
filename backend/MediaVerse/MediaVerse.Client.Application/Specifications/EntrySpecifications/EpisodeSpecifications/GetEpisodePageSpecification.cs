using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.EpisodeSpecifications;

public class GetEpisodePageSpecification : Specification<Entry>
{
    public GetEpisodePageSpecification(int page, int size, EntryOrder order, OrderDirection direction)
    {
        Query.Where(e => e.Episode != null)
            .Include(e => e.Episode)
            .IncludeEntry().OrderEntry(order, direction).Paginate(page, size);
    }
}