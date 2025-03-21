using MediatR;
using MediaVerse.Client.Application.Commands.ArticleCommands;
using MediaVerse.Client.Application.DTOs.ArticleDtos;
using MediaVerse.Client.Application.Queries.ArticleQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetArticles([FromQuery] GetArticlesQuery query)
    {
        return OkOrError(await mediator.Send(query));
    }
    
    [HttpPost]
    [Authorize("Journalist")]
    public async Task<IActionResult> AddArticle(CreateArticleCommand command)
    {
       var response = await mediator.Send(command);
       return ResolveCode(response.Exception, CreatedAtAction(nameof(GetArticle),new {id=response.Data?.Id},response.Data));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetArticle(Guid id)
    {
        var query = new GetArticleQuery(id);
        var response = await mediator.Send(query);
        return OkOrError(response);
    }
    [Authorize("Journalist")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateArticle(Guid id, UpdateArticleDto dto)
    {
        var command = new UpdateArticleCommand(id, dto);
        var response = await mediator.Send(command);
        return OkOrError(response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchArticles([FromQuery] SearchArticlesQuery query)
    {
        var response = await mediator.Send(query);
        return OkOrError(response);
    }
    
}