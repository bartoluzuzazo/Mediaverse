using MediatR;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("[controller]")]
[ApiController]
public class EntryController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetEntries([FromQuery] GetEntriesQuery query)
    {
        return Ok(await mediator.Send(query));
    }
}