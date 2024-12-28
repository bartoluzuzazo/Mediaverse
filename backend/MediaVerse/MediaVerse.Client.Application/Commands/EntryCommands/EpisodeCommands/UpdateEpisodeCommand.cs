using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.EpisodeDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.EpisodeSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.EntryCommands.EpisodeCommands;

public record UpdateEpisodeCommand(Guid Id, PatchEpisodeRequest Dto) : IRequest<BaseResponse<Guid>>;

public class UpdateEpisodeCommandHandler(
    IRepository<Episode> episodeRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> coverPhotoRepository,
    IRepository<AuthorRole> roleRepository,
    IRepository<WorkOn> workOnRepository,
    IMapper mapper)
    : UpdateEntryCommandHandler(entryRepository, coverPhotoRepository, roleRepository, workOnRepository, mapper),
        IRequestHandler<UpdateEpisodeCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(UpdateEpisodeCommand request, CancellationToken cancellationToken)
    {
        var command = new UpdateEntryCommand(request.Id, request.Dto.Entry);
        var response = await base.Handle(command, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<Guid>(response.Exception);
        
        var episodeSpecification = new GetEpisodeByIdSpecification(request.Id);
        var episode = await episodeRepository.FirstOrDefaultAsync(episodeSpecification, cancellationToken);

        episode.Synopsis = request.Dto.Synopsis ?? episode.Synopsis;
        episode.EpisodeNumber = request.Dto.EpisodeNumber ?? episode.EpisodeNumber;
        episode.SeasonNumber = request.Dto.SeasonNumber ?? episode.SeasonNumber;

        await episodeRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(episode.Id);
    }
}