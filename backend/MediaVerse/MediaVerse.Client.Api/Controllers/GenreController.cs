using MediatR;
using MediaVerse.Client.Application.Queries.GenresQueries;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController(IMediator mediator) : BaseController
{
    [HttpGet("search/book")]
    public async Task<IActionResult> SearchBookGenres(string query, int page, int size)
    {
        var request = new SearchBookGenresQuery(page, size, query);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
    
    [HttpGet("search/cinematic")]
    public async Task<IActionResult> SearchCinematicGenres(string query, int page, int size)
    {
        var request = new SearchCinematicGenresQuery(page, size, query);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
    
    [HttpGet("search/game")]
    public async Task<IActionResult> SearchGameGenres(string query, int page, int size)
    {
        var request = new SearchGameGenresQuery(page, size, query);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
    
    [HttpGet("search/music")]
    public async Task<IActionResult> SearchMusicGenres(string query, int page, int size)
    {
        var request = new SearchMusicGenresQuery(page, size, query);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
}