using Ardalis.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Specifications.RoleSpecifications;

namespace MediaVerse.Domain.Specifications.AuthorSpecifications;

public class GetAuthorWithPhotoSpecification : Specification<Author>
{
    public GetAuthorWithPhotoSpecification(Guid id)
    {
        Query.Where(a => a.Id.Equals(id)).Include(a => a.ProfilePicture);
    }
}