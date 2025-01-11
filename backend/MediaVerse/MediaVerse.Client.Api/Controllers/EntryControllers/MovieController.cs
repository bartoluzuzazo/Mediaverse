using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Client.Application.Commands.EntryCommands.MovieCommands;
using MediaVerse.Client.Application.DTOs.EntryDTOs.MovieDTOs;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Queries.EntryQueries.MovieQueries;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.MovieSpecifications;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("api/[controller]")]
[ApiController]
public class MovieController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize("ContentCreator")]
    public async Task<IActionResult> AddMovie(AddMovieCommand request)
    {
        var response = await mediator.Send(request);
        return CreatedOrError(response, nameof(GetMovie));
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize("ContentCreator")]
    public async Task<IActionResult> PatchMovie(Guid id, PatchMovieRequest request)
    {
        var command = new UpdateMovieCommand(id, request);
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception, Ok(nameof(GetMovie)));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMovie(Guid id)
    {
        var request = new GetMovieQuery(id);
        var response = await mediator.Send(request);
        return OkOrError(response);
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