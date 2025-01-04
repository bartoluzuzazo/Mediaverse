using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.SongSpecifications;

public class GetSongsByIdsSpecification : Specification<Song>
{
    public GetSongsByIdsSpecification(List<Guid> ids)
    {
        Query.Where(s => ids.Contains(s.Id));
    }
}