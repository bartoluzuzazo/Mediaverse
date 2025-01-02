using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.SongSpecifications;
using MediaVerse.Client.Application.Specifications.GenresSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands.AlbumCommands;

public record AddAlbumCommand(AddEntryCommand Entry, List<string>? Genres, List<Guid> SongIds) : IRequest<BaseResponse<Guid>>;

public class AddAlbumCommandHandler(
    IRepository<Album> albumRepository,
    IRepository<MusicGenre> musicGenreRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<WorkOn> workOnRepository,
    IRepository<AuthorRole> roleRepository,
    IRepository<Song> songRepository,
    IMapper mapper)
    : AddEntryCommandHandler(entryRepository, photoRepository, workOnRepository, roleRepository, mapper),
        IRequestHandler<AddAlbumCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(AddAlbumCommand request, CancellationToken cancellationToken)
    {
        var entryResponse = await base.Handle(request.Entry, cancellationToken);

        var album = new Album()
        {
            Id = entryResponse.Data!.EntryId,
            MusicGenres = new List<MusicGenre>()
        };

        if (!request.Genres.IsNullOrEmpty())
        {
            var genreSpec = new GetMusicGenresByNameSpecification(request.Genres!);
            var dbGenres = await musicGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Genres!.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new MusicGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await musicGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            album.MusicGenres = dbGenres;
        }
        
        if (!request.SongIds.IsNullOrEmpty())
        {
            var spec = new GetSongsByIdsSpecification(request.SongIds);
            var songs = await songRepository.ListAsync(spec, cancellationToken);
            album.Songs = songs;
        }

        await albumRepository.AddAsync(album, cancellationToken);

        return new BaseResponse<Guid>(album.Id);
    }
}