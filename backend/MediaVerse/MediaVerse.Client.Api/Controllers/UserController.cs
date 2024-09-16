﻿using MediatR;
using MediaVerse.Client.Application.Commands.UserCommands;
using MediaVerse.Client.Application.Queries.UserQueries;
using MediaVerse.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
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
        if (response.Exception is not null)
        {
            return response.Exception is ConflictException ? Conflict() : Problem(response.Exception.Message);
        }
        return CreatedAtAction(nameof(LoginUser), response.Data);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginUserQuery request)
    {
        var response = await mediator.Send(request);
        if (response.Exception is not null)
        {
            return response.Exception is NotFoundException ? NotFound() : Problem(response.Exception.Message);
        }

        return Ok(response.Data);
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var query = new GetUserQuery(userId);
        var response = await mediator.Send(query);
        return ResolveCode(response.Exception, Ok(response.Data));
    }

    [HttpGet("email/{userEmail}")]
    public async Task<IActionResult> GetUserByEmail(string userEmail)
    {
        var query = new GetUserByEmailQuery(userEmail);
        var response = await mediator.Send(query);
        return ResolveCode(response.Exception, Ok(response.Data));
    }

    [Authorize]
    [HttpPatch("current-user")]
    public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
    {
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception, Ok(response.Data));
    }
    
}