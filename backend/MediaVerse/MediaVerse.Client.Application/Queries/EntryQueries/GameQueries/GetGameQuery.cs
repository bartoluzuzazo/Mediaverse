using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.GameDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.GameSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.EntryQueries.GameQueries;

public record GetGameQuery(Guid Id) : IRequest<BaseResponse<GetGameResponse>>;

public class GetGameQueryHandler(IRepository<Game> gameRepository, IRepository<Entry> entryRepository)
    : GetBaseEntryQueryHandler(entryRepository), IRequestHandler<GetGameQuery, BaseResponse<GetGameResponse>>
{
    public async Task<BaseResponse<GetGameResponse>> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var query = new GetBaseEntryQuery(request.Id);
        var response = await base.Handle(query, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<GetGameResponse>(response.Exception);
        
        var spec = new GetGameByIdSpecification(request.Id);
        var game = await gameRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (game is null) return new BaseResponse<GetGameResponse>(new NotFoundException());

        var responseBook = new GetGameResponse(game.Synopsis, game.GameGenres.Select(bg => bg.Name).ToList(), response.Data);
        return new BaseResponse<GetGameResponse>(responseBook);
    }
}