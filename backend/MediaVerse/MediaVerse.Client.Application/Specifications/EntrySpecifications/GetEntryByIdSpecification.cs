using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetEntryByIdSpecification : Specification<Entry>
{
    public GetEntryByIdSpecification(Guid id)
    {
        Query.Where(e => e.Id == id)
            .Include(e => e.CoverPhoto)
            .Include(e => e.Ratings)
            .Include(e => e.WorkOns).ThenInclude(w => w.Author).ThenInclude(a => a.ProfilePicture)
            .Include(e => e.WorkOns).ThenInclude(w => w.AuthorRole);
    }
}