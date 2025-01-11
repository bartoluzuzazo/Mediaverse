using Ardalis.Specification;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetRatedEntriesSpecification : Specification<Entry, GetRatedEntryResponse>
{
    public GetRatedEntriesSpecification(Guid userId, int page, int size, RatedEntryOrder order,
        OrderDirection direction)
    {
        Query.Where(entry => entry.Ratings.Any(r => r.UserId == userId));
        Query.Select(entry => new()
        {
            Id = entry.Id,
            Name = entry.Name,
            Type = entry.Type!,
            Photo = Convert.ToBase64String(entry.CoverPhoto.Photo),
            UsersRating = entry.Ratings.FirstOrDefault(r => r.UserId == userId)!.Grade
        });
        switch (order)
        {
            case RatedEntryOrder.RatedByUserAt:
                Query.OrderByDirection(e => e.Ratings.FirstOrDefault(r => r.UserId == userId)!.Modifiedat, direction);
                break;
        }

        Query.Paginate(page, size);
    }
}