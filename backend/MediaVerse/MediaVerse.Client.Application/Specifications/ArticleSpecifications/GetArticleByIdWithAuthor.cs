
using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.ArticleSpecifications;

public class GetArticleByIdWithAuthor : Specification<Article>
{
    public GetArticleByIdWithAuthor(Guid id)
    {
        Query.Where(a => a.Id == id).Include(a=>a.User).ThenInclude(u=>u.ProfilePicture);
    }
}