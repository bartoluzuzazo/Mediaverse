using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.UserSpecifications;

public class GetUserByUsernameSpecification :Specification<User>
{
    public GetUserByUsernameSpecification(string username)
    {
        Query.Where(user => user.Username == username);
    }
}