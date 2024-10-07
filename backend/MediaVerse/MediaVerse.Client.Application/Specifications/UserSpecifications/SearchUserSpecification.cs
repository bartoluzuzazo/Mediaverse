using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.UserSpecifications;

public class SearchUserSpecification : Specification<User>
{
    public SearchUserSpecification(string searchTerm, int pageNumber, int pageSize)
    {
        Query.Search(user => user.Username, "%" + searchTerm + "%").Include(u=>u.ProfilePicture).Paginate(pageNumber, pageSize)
            .OrderBy(user => user.Id);
    }
}