using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands.AlbumCommands;
using MediaVerse.Client.Application.DTOs.EntryDTOs.AlbumDTOs;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Queries.EntryQueries.AlbumQueries;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.AlbumSpecifications;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("api/[controller]")]
[ApiController]
public class AlbumController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddAlbum(AddAlbumCommand request)
    {
        var response = await mediator.Send(request);
        return CreatedOrError(response, nameof(GetAlbum));
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> PatchAlbum(Guid id, PatchAlbumRequest request)
    {
        var command = new UpdateAlbumCommand(id, request);
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception, Ok(nameof(GetAlbum)));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAlbum(Guid id)
    {
        var request = new GetAlbumQuery(id);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
    
    [HttpGet("page")]
    public async Task<IActionResult> GetAlbums(int page, int size, EntryOrder order, OrderDirection direction)
    {
        var spec = new GetAlbumPageSpecification(page, size, order, direction);
        var request = new GetEntryPageQuery(spec);
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, Ok(response.Data));
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchAlbums(string query, int page, int size)
    {
        var request = new SearchAlbumsQuery(page, size, query);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
}