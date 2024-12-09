using MediatR;
using MediaVerse.Client.Application.Commands.ReviewCommands;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Queries.ReviewQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("api/[controller]")]
[ApiController]
public class EntryController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetEntries([FromQuery] GetEntriesQuery query)
    {
        return Ok(await mediator.Send(query));
    }
    
    [HttpGet("type/{id:guid}")]
    public async Task<IActionResult> GetEntryType(Guid id)
    {
        var query = new GetEntryTypeQuery(id);
        var response = await mediator.Send(query);
        return OkOrError(response);
    }

    [HttpPut("{id:guid}/reviews/current-user")]
    [Authorize]
    public async Task<IActionResult> PutReview(CreateOrUpdateReviewCommand command)
    {
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception, CreatedAtAction(nameof(GetReview), new {entryId= response.Data?.EntryId, userId=response.Data?.UserId}, response.Data));
    }

    [HttpGet("{entryId:guid}/reviews/{userId:guid}")]
    public async Task<IActionResult> GetReview(Guid entryId, Guid userId)
    {
        var query = new GetReviewQuery(entryId, userId);
        var response = await mediator.Send(query);
        return OkOrError(response);
    }
    
}