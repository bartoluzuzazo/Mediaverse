using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("[controller]")]
[ApiController]
public class BookController(IMediator mediator) : BaseController
{
    [HttpPost]
    // [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddBook(AddBookCommand request)
    {
        var response = await mediator.Send(request);
        if (request.WorkOnRequests.IsNullOrEmpty() || response.Data is null)
            return ResolveCode(response.Exception, CreatedAtAction(nameof(GetBook), response.Data));
        var addAuthorsRequest = new AddEntryAuthorsCommand(response.Data.Id, request.WorkOnRequests);
        await mediator.Send(addAuthorsRequest);
        return ResolveCode(response.Exception, CreatedAtAction(nameof(GetBook), response.Data));
    }
    
    [HttpPatch("{id:guid}")]
    // [Authorize(Policy = "Admin")]
    public async Task<IActionResult> PatchBook(Guid id, UpdateBookCommand request)
    {
        request.Id = id;
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, CreatedAtAction(nameof(GetBook), response.Data));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBook(Guid id)
    {
        var request = new GetBookQuery(id);
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, Ok(response.Data));
    }
    
    [HttpGet("page")]
    public async Task<IActionResult> GetBooks(int page, int size, EntryOrder order, OrderDirection direction)
    {
        var request = new GetBookPageQuery(page, size, order, direction);
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, Ok(response.Data));
    }
    
    [HttpPost("authors")]
    // [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddBookAuthors(AddEntryAuthorsCommand request)
    {
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, CreatedAtAction(nameof(GetBook), response.Data));
    }
}