using MediatR;
using MediaVerse.Client.Application.Commands.EntryCommands.EpisodeCommands;
using MediaVerse.Client.Application.DTOs.EntryDTOs.EpisodeDTOs;
using MediaVerse.Client.Application.Queries.EntryQueries;
using MediaVerse.Client.Application.Queries.EntryQueries.EpisodeQueries;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.EpisodeSpecifications;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers.EntryControllers;

[Route("api/[controller]")]
[ApiController]
public class EpisodeController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize("ContentCreator")]
    public async Task<IActionResult> AddEpisode(AddEpisodeCommand request)
    {
        var response = await mediator.Send(request);
        return CreatedOrError(response, nameof(GetEpisode));
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize("ContentCreator")]
    public async Task<IActionResult> PatchEpisode(Guid id, PatchEpisodeRequest request)
    {
        var command = new UpdateEpisodeCommand(id, request);
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception, Ok(nameof(GetEpisode)));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEpisode(Guid id)
    {
        var request = new GetEpisodeQuery(id);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
    
    [HttpGet("page")]
    public async Task<IActionResult> GetEpisodes(int page, int size, EntryOrder order, OrderDirection direction)
    {
        var spec = new GetEpisodePageSpecification(page, size, order, direction);
        var request = new GetEntryPageQuery(spec);
        var response = await mediator.Send(request);
        return ResolveCode(response.Exception, Ok(response.Data));
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchEpisodes(string query, int page, int size)
    {
        var request = new SearchEpisodesQuery(page, size, query);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
}