using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.RatingSpecifications;

public class GetRatingByIdsSpecification : Specification<Rating>
{
    public GetRatingByIdsSpecification(Guid userId, Guid entryId)
    {
        Query.Where(rating => rating.UserId == userId && rating.EntryId == entryId);
    }
}