using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.RoleSpecifications;

public class GetUserRoleSpecification : Specification<Role>
{
    public GetUserRoleSpecification()
    {
        Query.Where(r => r.Name == "User");
    }
}