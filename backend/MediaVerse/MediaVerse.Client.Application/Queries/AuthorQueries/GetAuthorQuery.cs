using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.Extensions.EntryExtensions;
using MediaVerse.Client.Application.Specifications.AuthorSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.AuthorQueries;

public record GetAuthorQuery(Guid Id) : IRequest<BaseResponse<GetAuthorResponse>>;

public class GetAuthorQueryHandler(IRepository<Author> authorRepository)
    : IRequestHandler<GetAuthorQuery, BaseResponse<GetAuthorResponse>>
{
    public async Task<BaseResponse<GetAuthorResponse>> Handle(GetAuthorQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new GetAuthorWithPhotoSpecification(request.Id);
        var author = await authorRepository.FirstOrDefaultAsync(specification, cancellationToken);
        if (author is null)
        {
            return new BaseResponse<GetAuthorResponse>(new NotFoundException());
        }
        
        var workons = author.WorkOns.GroupBy(wo => wo.AuthorRole.Name).Select(group =>
            {
                return new GetAuthorWorkOnsGroupResponse
                {
                    Role = group.Key,
                    Entries = group.Select(wo =>
                    {
                        var score = wo.Entry.Ratings.IsNullOrEmpty() ? 0m : wo.Entry.Ratings.Average(r => Convert.ToDecimal(r.Grade));
                        
                        return new GetAuthorWorkOnResponse
                        {
                            Id = wo.EntryId,
                            Name = wo.Entry.Name,
                            CoverPhoto = wo.Entry.CoverPhoto.Photo,
                            AvgRating = score,
                            Description = wo.Entry.Description,
                            Type = wo.Entry.Type!,
                            ReleaseDate = wo.Entry.Release
                        };
                    }).ToList()
                };
            })
            .ToList();

        var authorResponse = new GetAuthorResponse
        {
            Id = author.Id,
            Bio = author.Bio,
            Name = author.Name,
            Surname = author.Surname,
            WorkOns = workons,
            ProfilePicture = Convert.ToBase64String(author.ProfilePicture.Picture),
        };
        return new BaseResponse<GetAuthorResponse>(authorResponse);
    }
}