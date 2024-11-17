using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Client.Application.DTOs.EntryDTOs.MovieDTOs;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("[controller]")]
[ApiController]
public class MovieController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddMovie(AddMovieRequest request)
    {
        var entryResponse = await mediator.Send(request.Entry);
        var command = new AddMovieCommand(entryResponse.Data!.EntryId, request.Synopsis, request.Genres);
        var movieResponse = await mediator.Send(command);
        return ResolveCode(movieResponse.Exception, CreatedAtAction(nameof(GetMovie), new { id = entryResponse.Data.EntryId },new { id = entryResponse.Data.EntryId }));
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> PatchMovie(Guid id, PatchMovieRequest request)
    {
        var updateEntryCommand = new UpdateEntryCommand(id, request.Entry);
        var entryResponse = await mediator.Send(updateEntryCommand);
        var updateMovieCommand = new UpdateMovieCommand(id, request);
        var movieResponse = await mediator.Send(updateMovieCommand);
        return ResolveCode(movieResponse.Exception, Ok(nameof(GetMovie)));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMovie(Guid id)
    {
        var entryRequest = new GetBaseEntryQuery(id);
        var entryResponse = await mediator.Send(entryRequest);
        if (entryResponse.Exception is not null) return ResolveException(entryResponse.Exception);
        var movieRequest = new GetMovieQuery(id);
        var movieResponse = await mediator.Send(movieRequest);
        if (movieResponse.Exception is not null) return ResolveException(movieResponse.Exception);
        movieResponse.Data!.Entry = entryResponse.Data!;
        return Ok(movieResponse.Data);
    }
    
    [HttpGet("page")]
    public async Task<IActionResult> GetMovies(int page, int size, EntryOrder order, OrderDirection direction)
    {
        var spec = new GetMoviePageSpecification(page, size, order, direction);
        var request = new GetEntryPageQuery(spec);
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, Ok(response.Data));
    }
}