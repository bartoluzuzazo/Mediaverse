using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.RatingSpecifications;

public class GetRatingByEmailAndEntryId : Specification<Rating>
{
    public GetRatingByEmailAndEntryId(string userEmail, Guid entryId)
    {
        Query.Where(rating => rating.User.Email == userEmail && rating.EntryId == entryId);
    }
}