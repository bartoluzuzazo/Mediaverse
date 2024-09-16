using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("[controller]")]
[ApiController]
public class BookController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    // [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddBook(AddBookCommand request)
    {
        var response = await mediator.Send(request);
        if (response.Exception is not null) return Problem(response.Exception.Message);
        return CreatedAtAction(nameof(GetBook), response.Data);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBook(Guid id)
    {
        var request = new GetBookQuery(id);
        var response = await mediator.Send(request);
        if (response.Exception is not null)
        {
            return response.Exception is NotFoundException ? NotFound() : Problem(response.Exception.Message);
        }
        return Ok(response.Data);
    }
    
    [HttpGet("page")]
    public async Task<IActionResult> GetBooks(int page, int size, EntryOrder order, OrderDirection direction)
    {
        var request = new GetBookPageQuery(page, size, order, direction);
        var response = await mediator.Send(request);
        return Ok(response.Data);
    }
}