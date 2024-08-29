using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.RatingSpecifications;

public class GetRatingByUserNameSpecification : Specification<Rating>
{
    public GetRatingByUserNameSpecification(string userEmail)
    {
        Query.Where(rating => rating.User.Email == userEmail);
    }
}