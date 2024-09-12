using MediatR;
using MediaVerse.Client.Application.Commands.RatingCommands;
using MediaVerse.Client.Application.DTOs.RatingDTOs;
using MediaVerse.Client.Application.Queries.RatingQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[ApiController]
[Authorize]
[Route("")]
public class RatingsController(IMediator mediator) : BaseController
{
    [HttpGet("entries/{entryGuid}/ratings/users-rating")]
    public async Task<IActionResult> GetUsersRating(Guid entryGuid)
    {
        var query = new GetUsersRatingQuery(entryGuid);

        var result = await mediator.Send(query);

        if (result.Exception is not null)
        {
            return ResolveException(result.Exception);
        }

        return Ok(result.Data);
    }

    [HttpPost("entries/{entryGuid:guid}/ratings")]
    public async Task<IActionResult> CreateRating(Guid entryGuid,PostRatingDto postRatingDto)
    {
        var command = new CreateRatingCommand(entryGuid, postRatingDto);
        var result = await mediator.Send(command);

        if (result.Exception is not null)
        {
            return ResolveException(result.Exception);
        }

        return Created();
    }

    [HttpPut("ratings/{id:guid}")]
    public async Task<IActionResult> UpdateRating(Guid id, PutRatingDto ratingDto)
    {
        var updateRatingCommand = new UpdateRatingCommand(id, ratingDto);
        var result = await mediator.Send(updateRatingCommand);

        if (result.Exception is not null)
        {
            return ResolveException(result.Exception);
        }

        return Ok(result.Data);
    }
}