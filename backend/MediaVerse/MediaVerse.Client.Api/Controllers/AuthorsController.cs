using MediatR;
using MediaVerse.Client.Application.Commands.AuthorCommands;
using MediaVerse.Client.Application.Queries.AuthorQueries;
using MediaVerse.Domain.Exceptions;

namespace MediaVerse.Client.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthor([FromForm] CreateAuthorCommand createAuthorCommand)
    {
        var response = await _mediator.Send(createAuthorCommand);

        return Created(nameof(GetAuthor), new { Id = response.Data });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthor(Guid id)
    {
        var query = new GetAuthorQuery()
        {
            Id = id
        };
        var response = await _mediator.Send(query);
        if (response.Exception is NotFoundException)
        {
            return NotFound();
        }

        return Ok(response.Data);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchAuthor(Guid id, [FromForm] UpdateAuthorCommand command)
    {
        command.Id = id;
        var response = await _mediator.Send(command);
        if (response.Exception is NotFoundException)
        {
            return NotFound();
        }

        return Ok();
    }
}