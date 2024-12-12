using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.Specifications.GenresSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands.GameCommands;

public record AddGameCommand(AddEntryCommand Entry, string Synopsis, List<string>? Genres) : IRequest<BaseResponse<Guid>>;

public class AddGameCommandHandler(
    IRepository<Game> gameRepository,
    IRepository<GameGenre> gameGenreRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<WorkOn> workOnRepository,
    IRepository<AuthorRole> roleRepository,
    IMapper mapper)
    : AddEntryCommandHandler(entryRepository, photoRepository, workOnRepository, roleRepository, mapper),
        IRequestHandler<AddGameCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(AddGameCommand request, CancellationToken cancellationToken)
    {
        var entryResponse = await base.Handle(request.Entry, cancellationToken);

        var game = new Game()
        {
            Id = entryResponse.Data.EntryId,
            Synopsis = request.Synopsis,
            GameGenres = new List<GameGenre>()
        };

        if (!request.Genres.IsNullOrEmpty())
        {
            var genreSpec = new GetGameGenresByNameSpecification(request.Genres!);
            var dbGenres = await gameGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Genres!.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new GameGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await gameGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            game.GameGenres = dbGenres;
        }

        await gameRepository.AddAsync(game, cancellationToken);

        return new BaseResponse<Guid>(game.Id);
    }
}