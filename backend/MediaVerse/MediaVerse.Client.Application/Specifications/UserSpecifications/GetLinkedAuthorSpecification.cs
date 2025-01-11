using Ardalis.Specification;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.UserSpecifications;

public class GetLinkedAuthorSpecification : Specification<Author, GetEntryAuthorResponse>
{
    public GetLinkedAuthorSpecification(Guid userId)
    {
        Query.Where(author => author.UserId == userId).Include(author => author.ProfilePicture);
        Query.Select(author => new GetEntryAuthorResponse()
        {
            Id = author.Id,
            Name = author.Name,
            Surname = author.Surname,
            ProfilePicture = author.ProfilePicture.Picture,
        });
    }
}