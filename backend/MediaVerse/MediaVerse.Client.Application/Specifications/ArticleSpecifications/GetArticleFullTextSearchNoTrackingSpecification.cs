
using Ardalis.Specification;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaVerse.Client.Application.Specifications.ArticleSpecifications;

public class GetArticleFullTextSearchNoTrackingSpecification : Specification<Article>
{
    public GetArticleFullTextSearchNoTrackingSpecification(string searchTerm, int page, int size)
    {
        Query.Where(a => a.SearchVector != null &&
                         a.SearchVector.Matches(EF.Functions.ToTsQuery("english", searchTerm)))
            .OrderBy(a => a.SearchVector.Rank(EF.Functions.ToTsQuery("english", searchTerm)))
            .Include(a=>a.User).ThenInclude(u=>u.ProfilePicture)
            .Paginate(page, size);
        
        Query.AsNoTracking();
    }
}