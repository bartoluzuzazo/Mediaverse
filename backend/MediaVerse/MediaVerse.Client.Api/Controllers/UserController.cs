using MediatR;
using MediaVerse.Client.Application.Commands.UserCommands;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Queries.UserQueries;
using MediaVerse.Domain.ValueObjects.Enums;
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
        return ResolveCode(response.Exception, CreatedAtAction(nameof(LoginUser), response.Data));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginUserQuery request)
    {
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception,  Ok(response.Data));
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

    [Authorize]
    [HttpGet("current-user/friend-invites")]
    public async Task<IActionResult> GetInvites()
    {
        var query = new GetFriendInvitesQuery();
        var response = await mediator.Send(query);
        return OkOrError(response);
    }

    [HttpGet("{userId:guid}/rated-entries")]
    public async Task<IActionResult> GetRatedEntries(Guid userId, int page, int size, RatedEntryOrder order,
        OrderDirection direction)
    {
        var query = new GetRatedEntriesQuery(userId, page, size, order, direction);
        var response = await mediator.Send(query);
        return OkOrError(response);
    }

    [HttpGet("{userId:guid}/friends")]
    public async Task<IActionResult> GetFriends(Guid userId)
    {
        var query = new GetFriendsQuery(userId);
        var response = await mediator.Send(query);
        return OkOrError(response);
    }

    [HttpPut("current-user/password")]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordCommand command)
    {
        var response = await mediator.Send(command);
        return OkOrError(response);
    }
}