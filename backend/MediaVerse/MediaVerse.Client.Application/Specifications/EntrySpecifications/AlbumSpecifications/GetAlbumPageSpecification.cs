using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;

public class GetAlbumPageSpecification : Specification<Entry>
{
    public GetAlbumPageSpecification(int page, int size, EntryOrder order, OrderDirection direction)
    {
        Query.Where(e => e.Album != null)
            .Include(e => e.Album).ThenInclude(m => m!.MusicGenres)
            .IncludeEntry().OrderEntry(order, direction).Paginate(page, size);
    }
}