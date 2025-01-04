using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.SongSpecifications;

public class GetSongByIdSpecification : Specification<Song>
{
    public GetSongByIdSpecification(Guid id)
    {
        Query.Where(s => s.Id == id).Include(s => s.MusicGenres)
            .Include(s => s.Albums).ThenInclude(a => a.IdNavigation).ThenInclude(e => e.Ratings)
            .Include(s => s.Albums).ThenInclude(a => a.IdNavigation).ThenInclude(e => e.CoverPhoto);
    }
}