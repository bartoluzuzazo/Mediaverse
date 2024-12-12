using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.SeriesSpecifications;

public class GetSeriesByIdSpecification : Specification<Series>
{
    public GetSeriesByIdSpecification(Guid id) 
    {
        Query.Where(s => s.Id == id).Include(s => s.CinematicGenres).Include(s => s.Episodes).ThenInclude(ep => ep.IdNavigation).ThenInclude(e => e.Ratings);
    }
}