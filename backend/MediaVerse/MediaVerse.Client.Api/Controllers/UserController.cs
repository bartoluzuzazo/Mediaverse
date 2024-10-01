using MediatR;
using MediaVerse.Client.Application.Commands.UserCommands;
using MediaVerse.Client.Application.Queries.UserQueries;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController(IMediator mediator) : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterUserCommand request)
    {
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, CreatedAtAction(nameof(LoginUser), response.Data));
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginUserQuery request)
    {
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception,  Ok(response.Data));
    }
}