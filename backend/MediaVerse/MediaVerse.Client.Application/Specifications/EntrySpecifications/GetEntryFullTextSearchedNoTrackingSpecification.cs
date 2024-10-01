using Ardalis.Specification;
using MediaVerse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public sealed class GetEntryFullTextSearchedNoTrackingSpecification : Specification<Entry>
{
    public GetEntryFullTextSearchedNoTrackingSpecification(string searchTerm)
    {
        Query
            .Where(e => e.SearchVector.Matches(EF.Functions.ToTsQuery("english", searchTerm))
                        || EF.Functions.TrigramsSimilarity(searchTerm, e.Name) >= 0.2)
            .OrderByDescending(e => EF.Functions.ToTsVector(e.Name).Rank(EF.Functions.ToTsQuery("english", searchTerm)))
            .ThenByDescending(e =>
                EF.Functions.ToTsVector(e.Description).Rank(EF.Functions.ToTsQuery("english", searchTerm)))
            .ThenByDescending(e => EF.Functions.TrigramsSimilarity(searchTerm, e.Name));
    }
}