using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace MediaVerse.Client.Application.Queries.EntryQueries;

public record GetEntriesQuery(string SearchTerm) : IRequest<BaseResponse<List<GetEntryResponse>>>;

public class GetEntriesQueryHandler(IRepository<Entry> entryRepository, IMapper mapper)
    : IRequestHandler<GetEntriesQuery, BaseResponse<List<GetEntryResponse>>>
{
    public async Task<BaseResponse<List<GetEntryResponse>>> Handle(GetEntriesQuery request,
        CancellationToken cancellationToken)
    {
        var entryDbSet = entryRepository.GetDbSet();
        var entries = await entryDbSet.FromSqlInterpolated($"""
                                                            SELECT
                                                                * 
                                                            FROM
                                                            full_text_search_entries({request.SearchTerm})
                                                            LIMIT 5
                                                            """)
            .AsNoTracking().Include(e => e.CoverPhoto).Include(e => e.Ratings)
            .ToListAsync(cancellationToken);
        entries.Reverse();

        return new BaseResponse<List<GetEntryResponse>>(mapper.Map<List<GetEntryResponse>>(entries));
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