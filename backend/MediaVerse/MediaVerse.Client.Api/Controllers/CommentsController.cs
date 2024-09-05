using MediatR;
using MediaVerse.Client.Application.Commands.CommentCommands;
using MediaVerse.Client.Application.Queries.CommentQueries;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[ApiController]
[Route("")]
public class CommentsController : BaseController
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("comments/{commentId}/votes")]
    public async Task<IActionResult> PostVote(Guid commentId, CreateVoteCommand command)
    {
        command.CommentId = commentId;
        var result = await _mediator.Send(command);
        return ResolveCode(result.Exception, Created("", result.Data));
    }

    [HttpPut("comments/{commentId}/votes/current-user")]
    public async Task<IActionResult> PutVote(Guid commentId, UpdateVoteCommand command)
    {
        command.CommentId = commentId;
        var result = await _mediator.Send(command);
        return ResolveCode(result.Exception, Ok(result.Data));
    }

    [HttpDelete("comments/{commentId}/votes/current-user")]
    public async Task<IActionResult> DeleteVote(Guid commentId)
    {
        var command = new DeleteVoteCommand()
        {
            CommentId = commentId
        };
        var exception = await _mediator.Send(command);
        return ResolveCode(exception, Ok());
    }


    [HttpPost("entries/{entryId}/comments")]
    [Authorize]
    public async Task<IActionResult> PostTopLevelComment(Guid entryId, CreateTopLevelCommentCommand command)
    {
        command.EntryId = entryId;
        var result = await _mediator.Send(command);
        return ResolveCode(result.Exception, CreatedAtAction(nameof(GetComments), new { entryId }, result.Data));
    }

    [HttpPost("comments/{commentId}/sub-comments")]
    [Authorize]
    public async Task<IActionResult> PostSubComment(Guid commentId, CreateSubCommentCommand command)
    {
        command.ParentCommentId = commentId;
        var result = await _mediator.Send(command);
        return ResolveCode(result.Exception, CreatedAtAction(nameof(GetSubcomments), new { commentId }, result.Data));
    }

    [HttpGet("entries/{entryId}/authorized-comments")]
    [Authorize]
    public async Task<IActionResult> GetCommentsWithUsersVote(Guid entryId, int page, int size, CommentOrder order,
        OrderDirection direction)
    {
        var query = new GetTopLevelCommentsAuthorizedQuery()
        {
            EntryId = entryId,
            Page = page,
            Size = size,
            Order = order,
            Direction = direction
        };
        var result = await _mediator.Send(query);
        return ResolveCode(result.Exception, Ok(result.Data));
    }

    [HttpGet("entries/{entryId}/comments")]
    public async Task<IActionResult> GetComments(Guid entryId, int page, int size, CommentOrder order,
        OrderDirection direction)
    {
        var query = new GetTopLevelCommentsQuery()
        {
            EntryId = entryId,
            Page = page,
            Size = size,
            Order = order,
            Direction = direction
        };
        var result = await _mediator.Send(query);
        return ResolveCode(result.Exception, Ok(result.Data));
    }

    [HttpGet("comments/{commentId}/sub-comments")]
    public async Task<IActionResult> GetSubcomments(Guid commentId, int page, int size, CommentOrder order,
        OrderDirection direction)
    {
        var query = new GetTopLevelCommentsQuery()
        {
            ParentId = commentId,
            Page = page,
            Size = size,
            Order = order,
            Direction = direction
        };
        var result = await _mediator.Send(query);
        return ResolveCode(result.Exception, Ok(result.Data));
    }


    [HttpGet("comments/{commentId}/authorized-sub-comments")]
    [Authorize]
    public async Task<IActionResult> GetSubcommentsAuthorized(Guid commentId, int page, int size, CommentOrder order,
        OrderDirection direction)
    {
        var query = new GetTopLevelCommentsAuthorizedQuery()
        {
            ParentId = commentId,
            Page = page,
            Size = size,
            Order = order,
            Direction = direction
        };
        var result = await _mediator.Send(query);
        return ResolveCode(result.Exception, Ok(result.Data));
    }
}