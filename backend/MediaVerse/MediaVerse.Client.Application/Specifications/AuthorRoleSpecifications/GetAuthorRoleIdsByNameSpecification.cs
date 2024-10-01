using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.AuthorRoleSpecifications;

public class GetAuthorRoleIdsByNameSpecification : Specification<AuthorRole>
{
    public GetAuthorRoleIdsByNameSpecification(IEnumerable<string> roleNames)
    {
        Query.Where(ar => roleNames.Contains(ar.Name));
    }
}