
using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.AmaSpecifications;

public class GetQuestionWithLikesSpecification : Specification<AmaQuestion>
{
    public GetQuestionWithLikesSpecification(Guid id)
    {
        Query.Where(a => a.Id == id).Include(a=>a.Users);
    }
}