using Ardalis.Specification;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.EntryQueries;

public record GetEntryPageQuery(ISpecification<Entry> Specification) : IRequest<BaseResponse<GetEntryListPageResponse>>;

public class GetEntryPageQueryHandler(IRepository<Entry> entryRepository) : IRequestHandler<GetEntryPageQuery, BaseResponse<GetEntryListPageResponse>>
{
    public async Task<BaseResponse<GetEntryListPageResponse>> Handle(GetEntryPageQuery request, CancellationToken cancellationToken)
    {
        var list = await entryRepository.ListAsync(request.Specification, cancellationToken);

        var entries = list.Select(entry =>
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

        var response = new GetEntryListPageResponse(entries);
        
        return new BaseResponse<GetEntryListPageResponse>(response);
    }
}