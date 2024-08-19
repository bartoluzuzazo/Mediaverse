using MediatR;
using MediaVerse.Client.Application.Commands.AuthorCommands;

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

    [HttpGet]
    public async Task<IActionResult> GetAuthor()
    {
        throw new NotImplementedException();
    }
    
}