using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Specifications.AuthorSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.EntryQueries;

public record GetEntriesQuery(string SearchTerm, int? Page, string? Type) : IRequest<BaseResponse<GetEntriesQueryResponse>>;

public class GetEntriesQueryResponse
{
    public List<GetEntryResponse> Entries { get; set; }
    public List<GetAuthorResponse> Authors { get; set; }
}

public class GetEntriesQueryHandler(
    IRepository<Entry> entryRepository,
    IRepository<Author> authorRepository,
    IMapper mapper)
    : IRequestHandler<GetEntriesQuery, BaseResponse<GetEntriesQueryResponse>>
{
    public async Task<BaseResponse<GetEntriesQueryResponse>> Handle(GetEntriesQuery request,
        CancellationToken cancellationToken)
    {
        var entryDbSet = entryRepository.GetDbSet();

        FormattableString interpolatedString;

        if (request.Type is null)
        {
            interpolatedString = $"""
                                  SELECT
                                      * 
                                  FROM
                                  full_text_search_entries({request.SearchTerm})
                                  LIMIT 5
                                  OFFSET {(request.Page ?? 0) * 5}
                                  """;
        }
        else
        {
            interpolatedString = $"""
                                  SELECT
                                      * 
                                  FROM
                                  full_text_search_entries({request.SearchTerm}) e
                                  WHERE e.type = {request.Type}
                                  LIMIT 5
                                  OFFSET {(request.Page ?? 0) * 5}
                                  """;
        }
        
        var entries = await entryDbSet.FromSqlInterpolated(interpolatedString)
            .AsNoTracking().Include(e => e.CoverPhoto).Include(e => e.Ratings)
            .ToListAsync(cancellationToken);
        entries.Reverse();

        var authors =
            await authorRepository.ListAsync(new GetAuthorFullTextSearchNoTrackingSpecification(request.SearchTerm),
                cancellationToken);

        return new BaseResponse<GetEntriesQueryResponse>(new GetEntriesQueryResponse
        {
            Entries = mapper.Map<List<GetEntryResponse>>(entries),
            Authors = mapper.Map<List<GetAuthorResponse>>(authors)
        });
    }
}

public class EntryProfile : Profile
{
    public EntryProfile()
    {
        CreateMap<Entry, GetEntryResponse>().ForMember(d => d.Photo,
                opt => opt.MapFrom(s => Convert.ToBase64String(s.CoverPhoto.Photo)))
            .ForMember(d => d.RatingAvg,
                opt => opt.MapFrom(s =>
                    s.Ratings.IsNullOrEmpty() ? 0m : s.Ratings.Average(r => Convert.ToDecimal(r.Grade))));
    }
}