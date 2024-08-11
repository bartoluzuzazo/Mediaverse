using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Domain.Specifications.UserSpecifications;

public class UserExistsSpec : Specification<User>
{
    public UserExistsSpec(string username, string email)
    {
        Query.Where(u => u.Username == username || u.Email == email);
    }
}