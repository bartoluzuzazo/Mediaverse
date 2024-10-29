using MediatR;
using MediaVerse.Client.Application.Commands.AuthorCommands;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.Queries.AuthorQueries;
using Microsoft.AspNetCore.Authorization;

namespace MediaVerse.Client.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class AuthorsController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> CreateAuthor(CreateAuthorCommand createAuthorCommand)
    {
        var response = await mediator.Send(createAuthorCommand);
        return ResolveCode(response.Exception, CreatedAtAction(nameof(GetAuthor), new { id = response.Data },new { id = response.Data }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAuthor(Guid id)
    {
        var query = new GetAuthorQuery(id);
        var response = await mediator.Send(query);
        return ResolveCode(response.Exception, Ok(response.Data));
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PatchAuthor(Guid id, PatchAuthorDto authorDto)
    {
        var command = new UpdateAuthorCommand(id, authorDto);
        var response = await mediator.Send(command);
        return ResolveCode(response.Exception, Ok());
    }

    [HttpGet("{id:guid}/linked-user")]
    public async Task<IActionResult> GetLinkedUser(Guid id)
    {
        var query = new GetLinkedUserQuery(id);
        var response = await mediator.Send(query);
        return OkOrError(response);
        
    }
    //TODO: add post /id/linked-user
    [HttpPut("{id:guid}/linked-user")]
    public async Task<IActionResult> AddLinkToUser(Guid id, AddLinkedUserDto dto)
    {
        var command = new LinkUserToAuthorCommand(id,dto );
        var response = await mediator.Send(command);
        return OkOrError(response);
    }
    //TODO: add delete /id/linked-user
    
    [HttpDelete("{id:guid}/linked-user")]
    public async Task<IActionResult> RemoveLinkToUser(Guid id)
    {
        var command = new UnlinkUserFromAuthorCommand(id);
        var response = await mediator.Send(command);
        return OkOrError(response);
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchAuthors(string query, int page, int size)
    {
        var request = new SearchAuthorsQuery(page, size, query);
        var response = await mediator.Send(request);
        return OkOrError(response);
    }
}