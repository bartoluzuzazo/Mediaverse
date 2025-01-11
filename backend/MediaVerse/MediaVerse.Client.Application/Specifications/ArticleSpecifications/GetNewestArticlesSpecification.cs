using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.ArticleSpecifications;

public sealed class GetNewestArticlesSpecification : Specification<Article>
{
    public GetNewestArticlesSpecification()
    {
        Query.OrderByDescending(x => x.Timestamp).Take(5);
        Query.Include(x => x.User).ThenInclude(u=>u.ProfilePicture);
    }
}