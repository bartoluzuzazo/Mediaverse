using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.EpisodeDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.EpisodeSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.EntryQueries.EpisodeQueries;

public record GetEpisodeQuery(Guid Id) : IRequest<BaseResponse<GetEpisodeResponse>>;

public class GetEpisodeQueryHandler(IRepository<Episode> episodeRepository, IRepository<Entry> entryRepository)
    : GetBaseEntryQueryHandler(entryRepository), IRequestHandler<GetEpisodeQuery, BaseResponse<GetEpisodeResponse>>
{
    public async Task<BaseResponse<GetEpisodeResponse>> Handle(GetEpisodeQuery request, CancellationToken cancellationToken)
    {
        var query = new GetBaseEntryQuery(request.Id);
        var response = await base.Handle(query, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<GetEpisodeResponse>(response.Exception);
        
        var spec = new GetEpisodeByIdSpecification(request.Id);
        var episode = await episodeRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (episode is null) return new BaseResponse<GetEpisodeResponse>(new NotFoundException());

        var responseBook = new GetEpisodeResponse(episode.SeriesId, episode.Series.IdNavigation.Name, episode.Synopsis, episode.SeasonNumber, episode.EpisodeNumber, response.Data!);
        return new BaseResponse<GetEpisodeResponse>(responseBook);
    }
}