
using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.ReviewSpecifications;

public class GetReviewByIdsSpecification : Specification<Review>
{
    public GetReviewByIdsSpecification(Guid userId, Guid entryId)
    {
        Query.Where(r => r.EntryId == entryId && r.UserId == userId).Include(r=>r.User).ThenInclude(u=>u.ProfilePicture);
    }
}