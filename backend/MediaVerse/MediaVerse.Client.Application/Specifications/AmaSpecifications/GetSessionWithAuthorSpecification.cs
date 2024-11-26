
using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.AmaSpecifications;

public class GetSessionWithAuthorSpecification : Specification<AmaSession>
{
    public GetSessionWithAuthorSpecification(Guid id)
    {
        Query.Where(session => session.Id == id).Include(session => session.Author).ThenInclude(author => author.ProfilePicture);
    }
}