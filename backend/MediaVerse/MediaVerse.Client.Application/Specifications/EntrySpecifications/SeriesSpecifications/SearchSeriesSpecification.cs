using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.SeriesSpecifications;

public class SearchSeriesSpecification : Specification<Series>
{
    public SearchSeriesSpecification(string searchTerm, int pageNumber, int pageSize)
    {
        Query.Include(series => series.IdNavigation).ThenInclude(e => e.CoverPhoto)
            .Search(series => series.IdNavigation.Name.ToLower(), "%" + searchTerm.ToLower() + "%")
            .Paginate(pageNumber, pageSize)
            .OrderBy(series => series.IdNavigation.Name);
    }
}