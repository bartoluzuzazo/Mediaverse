using AutoMapper;
using MediatR;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.EntryCommands.EpisodeCommands;

public record AddEpisodeCommand(AddEntryCommand Entry, Guid SeriesId, int SeasonNumber, int EpisodeNumber, string Synopsis) : IRequest<BaseResponse<Guid>>;

public class AddEpisodeCommandHandler(
    IRepository<Episode> episodeRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<WorkOn> workOnRepository,
    IRepository<AuthorRole> roleRepository,
    IMapper mapper)
    : AddEntryCommandHandler(entryRepository, photoRepository, workOnRepository, roleRepository, mapper),
        IRequestHandler<AddEpisodeCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(AddEpisodeCommand request, CancellationToken cancellationToken)
    {
        var entryResponse = await base.Handle(request.Entry, cancellationToken);

        var episode = new Episode()
        {
            Id = entryResponse.Data.EntryId,
            Synopsis = request.Synopsis,
            SeriesId = request.SeriesId,
            SeasonNumber = request.SeasonNumber,
            EpisodeNumber = request.EpisodeNumber
        };

        await episodeRepository.AddAsync(episode, cancellationToken);

        return new BaseResponse<Guid>(episode.Id);
    }
}