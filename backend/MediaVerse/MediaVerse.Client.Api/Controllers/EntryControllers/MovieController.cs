using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("[controller]")]
[ApiController]
// [Authorize(Policy = "Admin")]
public class MovieController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddMovie(AddMovieCommand request)
    {
        var response = await mediator.Send(request);
        if (response.Exception is not null) return Problem(response.Exception.Message);
        return CreatedAtAction("nameof(GetMovie)", response.Data);
    }
    
    /*[HttpGet]
    public async Task<IActionResult> GetMovie(Guid id)
    {
        var request = new GetMovieQuery(id);
        var response = await mediator.Send(request);
        if (response.Exception is not null)
        {
            return response.Exception is NotFoundException ? NotFound() : Problem(response.Exception.Message);
        }
        return Ok(response.Data);
    }
    
    [HttpGet("page")]
    public async Task<IActionResult> GetMovies(int page, int size, EntryOrder order, OrderDirection direction)
    {
        
        var request = new GetMoviePageQuery(page, size, order, direction);
        var response = await mediator.Send(request);
        return Ok(response.Data);
    }*/
}