using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.UserSpecifications;

public class GetUserByIdWithPictureSpecification: Specification<User>
{
    public GetUserByIdWithPictureSpecification(Guid id)
    {
        Query.Where(user => user.Id == id).Include(user => user.ProfilePicture);
    }
}