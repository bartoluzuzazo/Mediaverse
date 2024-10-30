using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.AuthorSpecifications;

public class SearchAuthorSpecification : Specification<Author>
{
    public SearchAuthorSpecification(string searchTerm, int pageNumber, int pageSize)
    {
        Query.Search(author => author.Name.ToLower(), "%" + searchTerm.ToLower() + "%")
            .Search(author => author.Surname.ToLower(), "%" + searchTerm.ToLower() + "%")
            .Include(u=>u.ProfilePicture)
            .Paginate(pageNumber, pageSize)
            .OrderBy(author => author.Id);
    }
}