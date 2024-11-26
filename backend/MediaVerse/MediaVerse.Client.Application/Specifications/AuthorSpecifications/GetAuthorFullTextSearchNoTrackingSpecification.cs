using Ardalis.Specification;
using MediaVerse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaVerse.Client.Application.Specifications.AuthorSpecifications;

public sealed class GetAuthorFullTextSearchNoTrackingSpecification : Specification<Author>
{
    public GetAuthorFullTextSearchNoTrackingSpecification(string searchTerm)
    {
        Query
            .Where(e => e.SearchVector != null &&
                        e.SearchVector.Matches(EF.Functions.ToTsQuery("english", searchTerm)));
        
        Query.AsNoTracking();
    }
}