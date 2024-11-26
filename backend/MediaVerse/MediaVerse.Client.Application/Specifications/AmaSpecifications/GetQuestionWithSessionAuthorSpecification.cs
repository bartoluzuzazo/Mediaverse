
using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.AmaSpecifications;

public class GetQuestionWithSessionAuthorSpecification : Specification<AmaQuestion>
{
    public GetQuestionWithSessionAuthorSpecification(Guid questionId)
    {
        Query.Where(a => a.Id == questionId).Include(q=>q.AmaSession).ThenInclude(a=>a.Author);
    }
}