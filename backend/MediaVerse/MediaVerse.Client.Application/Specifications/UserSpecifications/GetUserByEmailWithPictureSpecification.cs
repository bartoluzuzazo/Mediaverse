using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.UserSpecifications;

public class GetUserByEmailWithPictureSpecification: Specification<User>
{
    public GetUserByEmailWithPictureSpecification(string email)
    {
        Query.Where(user => user.Email == email).Include(user=>user.ProfilePicture);
    }
}