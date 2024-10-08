using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.AuthorSpecifications;

public class GetAuthorWithUserSpecification : Specification<Author>
{
    public GetAuthorWithUserSpecification(Guid authorId)
    {
        Query.Where(author => author.Id == authorId).Include(author => author.User)
            .ThenInclude(user => user!.ProfilePicture);
    }
}