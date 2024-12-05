using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("api/[controller]")]
[ApiController]
public class BookController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddBook(AddBookCommand request)
    {
        var response = await mediator.Send(request);
        return CreatedOrError(response, nameof(GetBook));
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> PatchBook(Guid id, PatchBookRequest request)
    {
        var command = new UpdateBookCommand(id, request);
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception, Ok(nameof(GetBook)));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBook(Guid id)
    {
        var request = new GetBookQuery(id);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
    
    [HttpGet("page")]
    public async Task<IActionResult> GetBooks(int page, int size, EntryOrder order, OrderDirection direction)
    {
        var spec = new GetBookPageSpecification(page, size, order, direction);
        var request = new GetEntryPageQuery(spec);
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, Ok(response.Data));
    }
}