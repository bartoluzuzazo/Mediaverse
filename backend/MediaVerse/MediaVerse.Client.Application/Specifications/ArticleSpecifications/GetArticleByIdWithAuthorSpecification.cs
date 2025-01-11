
using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.ArticleSpecifications;

public class GetArticleByIdWithAuthorSpecification : Specification<Article>
{
    public GetArticleByIdWithAuthorSpecification(Guid id)
    {
        Query.Where(a => a.Id == id).Include(a=>a.User).ThenInclude(u=>u.ProfilePicture);
    }
}