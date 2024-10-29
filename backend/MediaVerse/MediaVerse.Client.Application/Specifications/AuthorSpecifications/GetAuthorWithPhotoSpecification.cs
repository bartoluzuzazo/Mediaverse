using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.AuthorSpecifications;

public class GetAuthorWithPhotoSpecification : Specification<Author>
{
    public GetAuthorWithPhotoSpecification(Guid id)
    {
        Query.Where(a => a.Id.Equals(id))
            .Include(a => a.ProfilePicture)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.Ratings)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.CoverPhoto)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.AuthorRole);
    }
}