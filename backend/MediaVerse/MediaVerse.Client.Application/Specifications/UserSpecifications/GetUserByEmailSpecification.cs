using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.UserSpecifications;

public class GetUserByEmailSpecification : Specification<User>
{
    public GetUserByEmailSpecification(string email)
    {
        Query.Where(user => user.Email == email);
    }
}