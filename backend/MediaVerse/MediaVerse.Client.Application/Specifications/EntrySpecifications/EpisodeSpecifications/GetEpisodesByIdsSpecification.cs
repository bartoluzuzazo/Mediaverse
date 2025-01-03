using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.EpisodeSpecifications;

public class GetEpisodesByIdsSpecification : Specification<Episode>
{
    public GetEpisodesByIdsSpecification(List<Guid> ids)
    {
        Query.Where(s => ids.Contains(s.Id));
    }
}