using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.GameDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.GameSpecifications;
using MediaVerse.Client.Application.Specifications.GenresSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.EntryCommands.GameCommands;

public record UpdateGameCommand(Guid Id, PatchGameRequest Dto) : IRequest<BaseResponse<Guid>>;

public class UpdateGameCommandHandler(
    IRepository<Game> gameRepository,
    IRepository<GameGenre> gameGenreRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> coverPhotoRepository,
    IRepository<AuthorRole> roleRepository,
    IRepository<WorkOn> workOnRepository,
    IMapper mapper)
    : UpdateEntryCommandHandler(entryRepository, coverPhotoRepository, roleRepository, workOnRepository, mapper),
        IRequestHandler<UpdateGameCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
    {
        var command = new UpdateEntryCommand(request.Id, request.Dto.Entry);
        var response = await base.Handle(command, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<Guid>(response.Exception);
        
        var gameSpecification = new GetGameByIdSpecification(request.Id);
        var game = await gameRepository.FirstOrDefaultAsync(gameSpecification, cancellationToken);

        game.Synopsis = request.Dto.Synopsis ?? game.Synopsis;

        if (request.Dto.Genres is not null)
        {
            var genreSpec = new GetGameGenresByNameSpecification(request.Dto.Genres);
            var dbGenres = await gameGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Dto.Genres.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new GameGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await gameGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            game.GameGenres = dbGenres;
        }

        await gameRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(game.Id);
    }
}