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

public record GetBookPageQuery(int Page, int Size, EntryOrder Order, OrderDirection Direction) : IRequest<BaseResponse<GetBookPageResponse>>;

public class GetBookPageQueryHandler(IRepository<Entry> entryRepository) : IRequestHandler<GetBookPageQuery, BaseResponse<GetBookPageResponse>>
{
    public async Task<BaseResponse<GetBookPageResponse>> Handle(GetBookPageQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetBookPageSpecification(request.Page, request.Size, request.Order, request.Direction);
        var list = await entryRepository.ListAsync(spec, cancellationToken);

        var books = list.Select(entry =>
        {
            var ratingAvg = entry.Ratings.IsNullOrEmpty() ? 0m : entry.Ratings.Average(r => Convert.ToDecimal(r.Grade));

            var responseEntry = new GetEntryPageResponse()
            {
                Id = entry.Id,
                Name = entry.Name,
                Photo = Convert.ToBase64String(entry.CoverPhoto.Photo),
                RatingAvg = ratingAvg,
            };
            
            return responseEntry;
        }).ToList();

        var response = new GetBookPageResponse()
        {
            Books = books
        };
        
        return new BaseResponse<GetBookPageResponse>(response);
    }
}