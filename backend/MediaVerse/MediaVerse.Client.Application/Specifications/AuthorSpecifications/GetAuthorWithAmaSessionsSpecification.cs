
using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.AuthorSpecifications;

public class GetAuthorWithAmaSessionsSpecification : Specification<Author>
{
    public GetAuthorWithAmaSessionsSpecification(Guid id)
    {
        Query.Where(author => author.Id == id).Include(author => author.AmaSessions);
    }
}