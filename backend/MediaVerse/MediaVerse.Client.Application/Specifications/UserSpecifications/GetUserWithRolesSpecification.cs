
using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.UserSpecifications;

public class GetUserWithRolesSpecification : Specification<User>
{
    public GetUserWithRolesSpecification(Guid userId)
    {
        Query.Where(u => u.Id == userId).Include(u => u.Roles);
    }
}