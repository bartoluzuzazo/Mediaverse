using System.Diagnostics;
using MediatR;
using MediaVerse.Client.Application.Commands.RatingCommands;
using MediaVerse.Client.Application.Queries.RatingQueries;
using MediaVerse.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[Route("entities/[controller]")]
[ApiController]
[Authorize]
public class RatingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RatingsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("users-rating")]
    public async Task<IActionResult> GetUsersRating(Guid entryGuid)
    {
        var query = new GetUsersRatingQuery(entryGuid);

        var result = await _mediator.Send(query);

        if (result.Exception is NotFoundException)
        {
            return NotFound();
        }

        if (result.Exception is ProblemException)
        {
            return Problem();
        }

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRating(CreateRatingCommand createRatingCommand)
    {
        var result = await _mediator.Send(createRatingCommand);

        if (result.Exception is NotFoundException)
        {
            return NotFound();
        }

        if (result.Exception is ProblemException)
        {
            return Problem();
        }

        if (result.Exception is ForbiddenException)
        {
            return Forbid();
        }

        if (result.Exception is ConflictException)
        {
            return Conflict();
        }

        return CreatedAtAction(nameof(GetUsersRating), result.Data);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRating(UpdateRatingCommand updateRatingCommand)
    {
        var result = await _mediator.Send(updateRatingCommand);

        if (result.Exception is ConflictException)
        {
            return Conflict();
        }

        if (result.Exception is NotFoundException)
        {
            return NotFound();
        }

        if (result.Exception is ProblemException)
        {
            return Problem();
        }

        if (result.Exception is ForbiddenException)
        {
            return Forbid();
        }

        return Ok(result.Data);
    }
}