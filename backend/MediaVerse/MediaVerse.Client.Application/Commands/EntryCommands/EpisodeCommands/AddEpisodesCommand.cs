using AutoMapper;
using MediatR;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.EntryCommands.EpisodeCommands;

public record AddEpisodesCommand(Guid SeriesId, List<AddEpisodeCommand> Episodes) : IRequest<BaseResponse<List<Guid>>>;

public class AddEpisodesCommandHandler(
    IRepository<Episode> episodeRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<WorkOn> workOnRepository,
    IRepository<AuthorRole> roleRepository,
    IMapper mapper)
    : AddEntryCommandHandler(entryRepository, photoRepository, workOnRepository, roleRepository, mapper),
        IRequestHandler<AddEpisodesCommand, BaseResponse<List<Guid>>>
{
    public async Task<BaseResponse<List<Guid>>> Handle(AddEpisodesCommand request, CancellationToken cancellationToken)
    {
        var episodes = new List<Episode>();
        
        foreach (var episodeRequest in request.Episodes)
        {
            var entryResponse = await base.Handle(episodeRequest.Entry, cancellationToken);
            var episode = new Episode()
            {
                Id = entryResponse.Data!.EntryId,
                Synopsis = episodeRequest.Synopsis,
                SeriesId = request.SeriesId,
                SeasonNumber = episodeRequest.SeasonNumber,
                EpisodeNumber = episodeRequest.EpisodeNumber
            };
            episodes.Add(episode);
        }
        await episodeRepository.AddRangeAsync(episodes, cancellationToken);
        return new BaseResponse<List<Guid>>(episodes.Select(e => e.Id).ToList());
    }
}