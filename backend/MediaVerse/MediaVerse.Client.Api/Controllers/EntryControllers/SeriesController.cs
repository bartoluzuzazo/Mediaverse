using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands;
using MediaVerse.Client.Application.Commands.EntryCommands.SeriesCommands;
using MediaVerse.Client.Application.DTOs.EntryDTOs.SeriesDTOs;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Queries.EntryQueries.SeriesQueries;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.SeriesSpecifications;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("api/[controller]")]
[ApiController]
public class SeriesController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddSeries(AddSeriesCommand request)
    {
        var response = await mediator.Send(request);
        return CreatedOrError(response, nameof(GetSeries));
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> PatchSeries(Guid id, PatchSeriesRequest request)
    {
        var command = new UpdateSeriesCommand(id, request);
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception, Ok(nameof(GetSeries)));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSeries(Guid id)
    {
        var request = new GetSeriesQuery(id);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
    
    [HttpGet("page")]
    public async Task<IActionResult> GetSeriesPage(int page, int size, EntryOrder order, OrderDirection direction)
    {
        var spec = new GetSeriesPageSpecification(page, size, order, direction);
        var request = new GetEntryPageQuery(spec);
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, Ok(response.Data));
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchSeries(string query, int page, int size)
    {
        var request = new SearchSeriesQuery(page, size, query);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
}