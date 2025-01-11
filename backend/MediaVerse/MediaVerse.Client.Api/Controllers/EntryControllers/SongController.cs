using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands.SongCommands;
using MediaVerse.Client.Application.DTOs.EntryDTOs.SongDTOs;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Queries.EntryQueries.SongQueries;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.SongSpecifications;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("api/[controller]")]
[ApiController]
public class SongController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize("ContentCreator")]
    public async Task<IActionResult> AddSong(AddSongCommand request)
    {
        var response = await mediator.Send(request);
        return CreatedOrError(response, nameof(GetSong));
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize("ContentCreator")]
    public async Task<IActionResult> PatchSong(Guid id, PatchSongRequest request)
    {
        var command = new UpdateSongCommand(id, request);
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception, Ok(nameof(GetSong)));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSong(Guid id)
    {
        var request = new GetSongQuery(id);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
    
    [HttpGet("page")]
    public async Task<IActionResult> GetSongs(int page, int size, EntryOrder order, OrderDirection direction)
    {
        var spec = new GetSongPageSpecification(page, size, order, direction);
        var request = new GetEntryPageQuery(spec);
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, Ok(response.Data));
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchSongs(string query, int page, int size)
    {
        var request = new SearchSongsQuery(page, size, query);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
}