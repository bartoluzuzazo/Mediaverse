using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;
using MediaVerse.Client.Application.Specifications.GenresSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands.SongCommands;

public record AddSongCommand(AddEntryCommand Entry, string? Lyrics, List<string>? Genres, List<Guid> AlbumIds) : IRequest<BaseResponse<Guid>>;

public class AddSongCommandHandler(
    IRepository<Song> songRepository,
    IRepository<Album> albumRepository,
    IRepository<MusicGenre> musicGenreRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<WorkOn> workOnRepository,
    IRepository<AuthorRole> roleRepository,
    IMapper mapper)
    : AddEntryCommandHandler(entryRepository, photoRepository, workOnRepository, roleRepository, mapper),
        IRequestHandler<AddSongCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(AddSongCommand request, CancellationToken cancellationToken)
    {
        var entryResponse = await base.Handle(request.Entry, cancellationToken);

        var song = new Song()
        {
            Id = entryResponse.Data.EntryId,
            Lyrics = request.Lyrics,
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
            song.MusicGenres = dbGenres;
        }

        if (!request.AlbumIds.IsNullOrEmpty())
        {
            var spec = new GetAlbumsByIdsSpecification(request.AlbumIds);
            var albums = await albumRepository.ListAsync(spec, cancellationToken);
            song.Albums = albums;
        }
        
        await songRepository.AddAsync(song, cancellationToken);

        return new BaseResponse<Guid>(song.Id);
    }
}