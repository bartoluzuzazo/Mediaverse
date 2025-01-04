
using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.ReviewSpecifications;

public class GetReviewsByEntryIdWithCritic : Specification<Review>
{
    public GetReviewsByEntryIdWithCritic(Guid entryId)
    {
        Query.Where(r => r.EntryId == entryId)
            .Include(r => r.User)
            .ThenInclude(u=>u.ProfilePicture);
    }
}