using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetMoviePageSpecification : Specification<Entry>
{
    public GetMoviePageSpecification(int page, int size, EntryOrder order, OrderDirection direction)
    {
        Query.Where(e => e.Movie != null)
            .Include(e => e.Movie).ThenInclude(m => m!.CinematicGenres)
            .IncludeEntry().OrderEntry(order, direction).Paginate(page, size);
    }
}