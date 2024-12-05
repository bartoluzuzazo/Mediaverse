using MediatR;
using MediaVerse.Client.Application.Queries.EntryQueries;
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
}