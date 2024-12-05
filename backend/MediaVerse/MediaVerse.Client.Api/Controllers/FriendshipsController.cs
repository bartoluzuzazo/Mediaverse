using MediatR;
using MediaVerse.Client.Application.Commands.FriendshipCommands;
using MediaVerse.Client.Application.Queries.FriendshipQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FriendshipsController(IMediator mediator) : BaseController
{
    [HttpPost("{userId:guid}")]
    public async Task<IActionResult> CreateFriendship(Guid userId)
    {
        var command = new CreateFriendshipCommand(userId);
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception,
            CreatedAtAction(nameof(GetFriendship), new { userId = response.Data?.User2Id }, response.Data));
    }

    [HttpPost("{userId:guid}/approval")]
    public async Task<IActionResult> ApproveFriendship(Guid userId)
    {
        var command = new ApproveFriendshipCommand(userId);
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception,
            CreatedAtAction(nameof(GetFriendship), new {userId = response.Data?.UserId},response.Data));
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetFriendship(Guid userId)
    {
        var query = new GetFriendshipQuery(userId);
        var response = await mediator.Send(query);
        return ResolveCode(response.Exception, Ok(response.Data));
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteFriendship(Guid userId)
    {
        var command = new DeleteFriendshipCommand(userId);
        var response = await mediator.Send(command);
        return ResolveCode(response, Ok());
    }
}