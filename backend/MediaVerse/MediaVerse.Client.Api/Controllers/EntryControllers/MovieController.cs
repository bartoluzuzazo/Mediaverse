using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Client.Application.DTOs.EntryDTOs.MovieDTOs;
using MediaVerse.Client.Application.Queries.EntryQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("[controller]")]
[ApiController]
public class MovieController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddBook(AddMovieRequest request)
    {
        
        var entryResponse = await mediator.Send(request.Entry);
        var command = new AddMovieCommand(entryResponse.Data.EntryId, request.Synopsis, request.Genres);
        var movieResponse = await mediator.Send(command);
        return ResolveCode(movieResponse.Exception, CreatedAtAction(nameof(GetMovie), entryResponse.Data, entryResponse.Data));
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
        return Ok(movieResponse);
    }
}