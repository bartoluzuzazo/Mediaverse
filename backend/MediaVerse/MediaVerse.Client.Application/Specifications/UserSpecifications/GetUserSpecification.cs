using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.UserSpecifications;

public class GetUserSpecification : Specification<User>
{
    public GetUserSpecification(string email)
    {
        Query.Where(u => u.Email == email).Include(u => u.Roles);
    }
}