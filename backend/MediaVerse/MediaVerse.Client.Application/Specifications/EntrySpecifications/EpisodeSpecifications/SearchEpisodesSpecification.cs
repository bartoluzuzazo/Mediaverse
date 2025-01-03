using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.EpisodeSpecifications;

public class SearchEpisodesSpecification : Specification<Episode>
{
    public SearchEpisodesSpecification(string searchTerm, int pageNumber, int pageSize)
    {
        Query.Include(episode => episode.IdNavigation).ThenInclude(e => e.CoverPhoto)
            .Search(episode => episode.IdNavigation.Name.ToLower(), "%" + searchTerm.ToLower() + "%")
            .Paginate(pageNumber, pageSize)
            .OrderBy(episode => episode.IdNavigation.Name);
    }
}