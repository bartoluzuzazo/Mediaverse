using MediatR;
using MediaVerse.Client.Application.Commands.AuthorCommands;
using MediaVerse.Client.Application.Queries.AuthorQueries;
using Microsoft.AspNetCore.Authorization;

namespace MediaVerse.Client.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class AuthorsController(IMediator mediator) : BaseController
{
    [HttpPost]
    // [Authorize(Policy = "Admin")]
    public async Task<IActionResult> CreateAuthor(CreateAuthorCommand createAuthorCommand)
    {
        var response = await mediator.Send(createAuthorCommand);
        return ResolveCode(response.Exception, CreatedAtAction(nameof(GetAuthor), new { Id = response.Data }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAuthor(Guid id)
    {
        var query = new GetAuthorQuery(id);
        var response = await mediator.Send(query);
        return ResolveCode(response.Exception, Ok(response.Data));
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PatchAuthor(Guid id, UpdateAuthorCommand command)
    {
        command.Id = id;
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception, Ok());
    }
    
    [HttpPost("entries")]
    // [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddBookAuthors(AddAuthorEntriesCommand request)
    {
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, CreatedAtAction(nameof(GetAuthor), response.Data));
    }
}