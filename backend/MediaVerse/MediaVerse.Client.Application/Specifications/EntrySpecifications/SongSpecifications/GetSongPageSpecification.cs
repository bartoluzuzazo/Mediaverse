using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.SongSpecifications;

public class GetSongPageSpecification : Specification<Entry>
{
    public GetSongPageSpecification(int page, int size, EntryOrder order, OrderDirection direction)
    {
        Query.Where(e => e.Song != null)
            .Include(e => e.Song).ThenInclude(m => m!.MusicGenres)
            .IncludeEntry().OrderEntry(order, direction).Paginate(page, size);
    }
}