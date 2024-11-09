using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("[controller]")]
[ApiController]
public class BookController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddBook(AddBookRequest request)
    {
        var entryCommand = new AddEntryCommand(request.Name, request.Description, request.Release, request.CoverPhoto, request.WorkOnRequests);
        var entryResponse = await mediator.Send(entryCommand);
        var bookCommand = new AddBookCommand(entryResponse.Data!.EntryId, request.Isbn, request.Synopsis, request.Genres);
        var bookResponse = await mediator.Send(bookCommand);
        return ResolveCode(entryResponse.Exception, CreatedAtAction(nameof(GetBook), entryResponse.Data, entryResponse.Data));
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
        var entryRequest = new GetBaseEntryQuery(id);
        var entryResponse = await mediator.Send(entryRequest);
        if (entryResponse.Exception is not null) return ResolveException(entryResponse.Exception);
        var bookRequest = new GetBookQuery(id);
        var bookResponse = await mediator.Send(bookRequest);
        if (bookResponse.Exception is not null) return ResolveException(bookResponse.Exception);
        bookResponse.Data!.Entry = entryResponse.Data!;
        return Ok(bookResponse.Data);
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