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

public record GetBookQuery(Guid Id) : IRequest<BaseResponse<GetBookResponse>>;

public class GetBookQueryHandler(IRepository<Entry> entryRepository)
    : IRequestHandler<GetBookQuery, BaseResponse<GetBookResponse>>
{
    public async Task<BaseResponse<GetBookResponse>> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetBookByIdSpecification(request.Id);
        var entry = await entryRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (entry is null) return new BaseResponse<GetBookResponse>(new NotFoundException());

        var ratingAvg = entry.Ratings.IsNullOrEmpty() ? 0m : entry.Ratings.Average(r => Convert.ToDecimal(r.Grade));

        var responseEntry = new GetEntryResponse()
        {
            Id = entry.Id,
            Name = entry.Name,
            Description = entry.Description,
            Release = entry.Release,
            Photo = entry.CoverPhoto.Photo,
            RatingAvg = ratingAvg,
            Authors = entry.WorkOns.Select(wo => new GetEntryAuthorResponse()
            {
                Id = wo.Author.Id,
                Name = wo.Author.Name,
                Surname = wo.Author.Surname,
                Role = wo.AuthorRole.Name
            }).ToList()
        };
        
        var responseBook = new GetBookResponse()
        {
            Entry = responseEntry,
            Isbn = entry.Book.Isbn,
            Synopsis = entry.Book.Synopsis,
            BookGenres = entry.Book.BookGenres.Select(bg => bg.Name).ToList()
        };
        return new BaseResponse<GetBookResponse>(responseBook);
    }
}