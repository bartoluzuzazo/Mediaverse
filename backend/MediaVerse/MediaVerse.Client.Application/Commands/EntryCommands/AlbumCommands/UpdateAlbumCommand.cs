using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.AlbumDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.SongSpecifications;
using MediaVerse.Client.Application.Specifications.GenresSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands.AlbumCommands;

public record UpdateAlbumCommand(Guid Id, PatchAlbumRequest Dto) : IRequest<BaseResponse<Guid>>;

public class UpdateAlbumCommandHandler(
    IRepository<Album> albumRepository,
    IRepository<Song> songRepository,
    IRepository<MusicGenre> musicGenreRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> coverPhotoRepository,
    IRepository<AuthorRole> roleRepository,
    IRepository<WorkOn> workOnRepository,
    IMapper mapper)
    : UpdateEntryCommandHandler(entryRepository, coverPhotoRepository, roleRepository, workOnRepository, mapper),
        IRequestHandler<UpdateAlbumCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
    {
        var command = new UpdateEntryCommand(request.Id, request.Dto.Entry);
        var response = await base.Handle(command, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<Guid>(response.Exception);
        
        var albumSpecification = new GetAlbumByIdSpecification(request.Id);
        var album = await albumRepository.FirstOrDefaultAsync(albumSpecification, cancellationToken);

        if (request.Dto.Genres is not null)
        {
            var genreSpec = new GetMusicGenresByNameSpecification(request.Dto.Genres);
            var dbGenres = await musicGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Dto.Genres.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new MusicGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await musicGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            album.MusicGenres = dbGenres;
        }

        if (request.Dto.SongIds is not null)
        {
            var spec = new GetSongsByIdsSpecification(request.Dto.SongIds!);
            var songs = await songRepository.ListAsync(spec, cancellationToken);
            album.Songs = songs;
        }
        
        await albumRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(album.Id);
    }
}