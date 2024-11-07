using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.EntryQueries;

public record GetBaseEntryQuery(Guid Id) : IRequest<BaseResponse<GetEntryResponse>>;

public class GetBaseEntryQueryHandler(IRepository<Entry> entryRepository)
    : IRequestHandler<GetBaseEntryQuery, BaseResponse<GetEntryResponse>>
{
    public async Task<BaseResponse<GetEntryResponse>> Handle(GetBaseEntryQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetEntryByIdSpecification(request.Id);
        var entry = await entryRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (entry is null) return new BaseResponse<GetEntryResponse>(new NotFoundException());

        var ratingAvg = entry.Ratings.IsNullOrEmpty() ? 0m : entry.Ratings.Average(r => Convert.ToDecimal(r.Grade));

        var workons = entry.WorkOns.GroupBy(wo => wo.AuthorRole.Name)
            .Select(group => new GetEntryAuthorGroupResponse()
            {
                Role = group.Key,
                Authors = group.Select(wo => new GetEntryAuthorResponse()
                {
                    Id = wo.Author.Id,
                    Name = wo.Author.Name,
                    Surname = wo.Author.Surname,
                    ProfilePicture = wo.Author.ProfilePicture.Picture
                }).ToList()
            }).ToList();
        
        var responseEntry = new GetEntryResponse()
        {
            Id = entry.Id,
            Name = entry.Name,
            Description = entry.Description,
            Release = entry.Release,
            Photo = Convert.ToBase64String(entry.CoverPhoto.Photo),
            RatingAvg = ratingAvg,
            Authors = workons
        };
        
        return new BaseResponse<GetEntryResponse>(responseEntry);
    }
}