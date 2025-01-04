using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.SongDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.SongSpecifications;
using MediaVerse.Client.Application.Specifications.GenresSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands.SongCommands;

public record UpdateSongCommand(Guid Id, PatchSongRequest Dto) : IRequest<BaseResponse<Guid>>;

public class UpdateSongCommandHandler(
    IRepository<Song> songRepository,
    IRepository<Album> albumRepository,
    IRepository<MusicGenre> musicGenreRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> coverPhotoRepository,
    IRepository<AuthorRole> roleRepository,
    IRepository<WorkOn> workOnRepository,
    IMapper mapper)
    : UpdateEntryCommandHandler(entryRepository, coverPhotoRepository, roleRepository, workOnRepository, mapper),
        IRequestHandler<UpdateSongCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
    {
        var command = new UpdateEntryCommand(request.Id, request.Dto.Entry);
        var response = await base.Handle(command, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<Guid>(response.Exception);
        
        var songSpecification = new GetSongByIdSpecification(request.Id);
        var song = await songRepository.FirstOrDefaultAsync(songSpecification, cancellationToken);

        song.Lyrics = request.Dto.Lyrics ?? song.Lyrics;

        if (request.Dto.Genres is not null)
        {
            var genreSpec = new GetMusicGenresByNameSpecification(request.Dto.Genres);
            var dbGenres = await musicGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Dto.Genres.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new MusicGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await musicGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            song.MusicGenres = dbGenres;
        }

        if (request.Dto.AlbumIds is not null)
        {
            var spec = new GetAlbumsByIdsSpecification(request.Dto.AlbumIds!);
            var albums = await albumRepository.ListAsync(spec, cancellationToken);
            song.Albums = albums;
        }
        
        await songRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(song.Id);
    }
}