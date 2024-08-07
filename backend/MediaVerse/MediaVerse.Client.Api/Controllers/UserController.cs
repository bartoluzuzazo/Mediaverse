using MediatR;
using MediaVerse.Client.Application.Commands.UserCommands;
using MediaVerse.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterUserCommand request)
    {
        var response = await _mediator.Send(request);
        if (response.Exception is not null)
        {
            return response.Exception is ConflictException ? Conflict(response.Exception) : Problem(response.Exception.ToString());
        }
        return CreatedAtAction(nameof(LoginUser), response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser()
    {
        return Ok();
    }
}