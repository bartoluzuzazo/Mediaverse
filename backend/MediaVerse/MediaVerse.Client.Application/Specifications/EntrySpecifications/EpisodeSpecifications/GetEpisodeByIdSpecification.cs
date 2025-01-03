using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.EpisodeSpecifications;

public class GetEpisodeByIdSpecification : Specification<Episode>
{
    public GetEpisodeByIdSpecification(Guid id)
    {
        Query.Where(e => e.Id == id).Include(e => e.Series).ThenInclude(s => s.IdNavigation)
            .Include(e => e.Series).ThenInclude(s => s.IdNavigation).ThenInclude(e => e.Ratings)
            .Include(e => e.Series).ThenInclude(s => s.IdNavigation).ThenInclude(e => e.CoverPhoto);
    }
}