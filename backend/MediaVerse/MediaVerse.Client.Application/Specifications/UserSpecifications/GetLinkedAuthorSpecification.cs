using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.UserSpecifications;

public class GetLinkedAuthorSpecification : Specification<Author>
{
    public GetLinkedAuthorSpecification(Guid userId)
    {
        Query.Where(author => author.UserId == userId).Include(author => author.ProfilePicture);
    }
}