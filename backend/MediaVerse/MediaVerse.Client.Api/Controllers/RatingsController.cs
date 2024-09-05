using MediatR;
using MediaVerse.Client.Application.Commands.RatingCommands;
using MediaVerse.Client.Application.Queries.RatingQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[ApiController]
[Authorize]
[Route("")]
public class RatingsController(IMediator mediator) : BaseController
{
    [HttpGet("entries/{entryGuid:guid}/ratings/users-rating")]
    public async Task<IActionResult> GetUsersRating(Guid entryGuid)
    {
        var query = new GetUsersRatingQuery(entryGuid);
        var result = await mediator.Send(query);
        return ResolveCode(result.Exception, Ok(result.Data));
    }

    [HttpPost("entries/{entryGuid:guid}/ratings")]
    public async Task<IActionResult> CreateRating(Guid entryGuid, CreateRatingCommand createRatingCommand)
    {
        var result = await mediator.Send(createRatingCommand);
        return ResolveCode(result.Exception, CreatedAtAction(nameof(GetUsersRating), result.Data));
    }

    [HttpPut("ratings/{entryGuid:guid}")]
    public async Task<IActionResult> UpdateRating(Guid entryGuid, UpdateRatingCommand updateRatingCommand)
    {
        var result = await mediator.Send(updateRatingCommand);
        return ResolveCode(result.Exception, Ok(result.Data));
    }
}