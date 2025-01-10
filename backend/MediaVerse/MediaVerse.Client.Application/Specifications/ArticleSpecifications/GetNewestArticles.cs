using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.ArticleSpecifications;

public sealed class GetNewestArticles : Specification<Article>
{
    public GetNewestArticles()
    {
        Query.OrderByDescending(x => x.Timestamp).Take(5);
        Query.Include(x => x.User).ThenInclude(u=>u.ProfilePicture);
    }
}