using MediatR;
using MediaVerse.Client.Application.Commands.RatingCommands;
using MediaVerse.Client.Application.Queries.RatingQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[ApiController]
[Authorize]
[Route("")]
public class RatingsController : BaseController
{
    private readonly IMediator _mediator;

    public RatingsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("entries/{entryGuid}/ratings/users-rating")]
    public async Task<IActionResult> GetUsersRating(Guid entryGuid)
    {
        var query = new GetUsersRatingQuery(entryGuid);

        var result = await _mediator.Send(query);

        if (result.Exception is not null)
        {
            return ResolveException(result.Exception);
        }

        return Ok(result.Data);
    }

    [HttpPost("entries/{entryGuid}/ratings")]
    public async Task<IActionResult> CreateRating(CreateRatingCommand createRatingCommand)
    {
        var result = await _mediator.Send(createRatingCommand);

        if (result.Exception is not null)
        {
            return ResolveException(result.Exception);
        }

        return CreatedAtAction(nameof(GetUsersRating), result.Data);
    }

    [HttpPut("ratings/{id}")]
    public async Task<IActionResult> UpdateRating(Guid id, UpdateRatingCommand updateRatingCommand)
    {
        var result = await _mediator.Send(updateRatingCommand);

        if (result.Exception is not null)
        {
            return ResolveException(result.Exception);
        }

        return Ok(result.Data);
    }
}