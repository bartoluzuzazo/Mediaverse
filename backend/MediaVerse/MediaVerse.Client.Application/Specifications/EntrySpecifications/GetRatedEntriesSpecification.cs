using Ardalis.Specification;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetRatedEntriesSpecification :Specification<Entry,GetRatedEntryResponse>
{
    public GetRatedEntriesSpecification(Guid userId,int page, int size, EntryOrder entryOrder, OrderDirection direction )
    {
        Query.Where(entry => entry.Ratings.Any(r => r.UserId == userId));
        Query.Select(entry => new()
        {
            Id = entry.Id,
            Name = entry.Name,
            Photo = Convert.ToBase64String(entry.CoverPhoto.Photo),
            UsersRating = entry.Ratings.FirstOrDefault(r => r.UserId == userId)!.Grade
        });
        Query.OrderEntry(entryOrder,direction).Paginate(page, size);
    }
}