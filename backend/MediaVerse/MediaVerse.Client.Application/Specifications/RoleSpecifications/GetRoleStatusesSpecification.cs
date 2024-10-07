
using Ardalis.Specification;
using MediaVerse.Client.Application.DTOs.RoleDTOs;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.RoleSpecifications;

public class GetRoleStatusesSpecification : Specification<Role, GetRoleStatusResponse>
{
    public GetRoleStatusesSpecification(Guid userId)
    {
        Query
            .Select(role=> new GetRoleStatusResponse
            {
                Id=role.Id,
                Name=role.Name,
                IsUsers = role.Users.Any(u => u.Id == userId)
            });
    }
}
