using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.EntryQueries;

public record GetBookPageQuery(int page, int size, EntryOrder order, OrderDirection direction) : IRequest<BaseResponse<GetBookPageResponse>>;

public class GetBookPageQueryHandler(IRepository<Entry> entryRepository) : IRequestHandler<GetBookPageQuery, BaseResponse<GetBookPageResponse>>
{
    public async Task<BaseResponse<GetBookPageResponse>> Handle(GetBookPageQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetBookPageSpecification(request.page, request.size, request.order, request.direction);
        var list = await entryRepository.ListAsync(spec, cancellationToken);

        var books = list.Select(entry =>
        {
            var ratingAvg = entry.Ratings.IsNullOrEmpty() ? 0m : entry.Ratings.Select(r => Convert.ToDecimal(r.Grade)).Average();

            var responseEntry = new GetEntryResponse()
            {
                Id = entry.Id,
                Name = entry.Name,
                Description = entry.Description,
                Release = entry.Release,
                Photo = entry.CoverPhoto.Photo,
                RatingAvg = ratingAvg,
                Authors = entry.WorkOns.Select(wo => (wo.Author, wo.AuthorRole)).Select(ar =>
                    new GetEntryAuthorResponse()
                    {
                        Id = ar.Author.Id,
                        Name = ar.Author.Name,
                        Surname = ar.Author.Surname,
                        Role = ar.AuthorRole.Name
                    }).ToList()
            };

            var responseBook = new GetBookResponse()
            {
                Entry = responseEntry,
                Isbn = entry.Book.Isbn,
                Synopsis = entry.Book.Synopsis,
                BookGenres = entry.Book.BookGenres.Select(bg => bg.Name).ToList()
            };
            return responseBook;
        }).ToList();

        var response = new GetBookPageResponse()
        {
            Books = books
        };
        
        return new BaseResponse<GetBookPageResponse>(response);
    }
}