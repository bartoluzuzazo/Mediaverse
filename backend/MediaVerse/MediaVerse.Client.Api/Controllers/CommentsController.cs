using MediatR;
using MediaVerse.Client.Application.Commands.CommentCommands;
using MediaVerse.Client.Application.DTOs.CommentDtos;
using MediaVerse.Client.Application.Queries.CommentQueries;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[ApiController]
[Route("api/")]
public class CommentsController(IMediator mediator) : BaseController
{
    [HttpPost("comments/{commentId:guid}/votes")]
    public async Task<IActionResult> PostVote(Guid commentId, PostVoteDto voteDto)
    {
        var command = new CreateVoteCommand(commentId, voteDto);
        var result = await mediator.Send(command);
        return ResolveCode(result.Exception, Created("", result.Data));
    }

    [HttpPut("comments/{commentId:guid}/votes/current-user")]
    public async Task<IActionResult> PutVote(Guid commentId, PutVoteDto voteDto)
    {
        var command = new UpdateVoteCommand()
        {
            CommentId = commentId,
            VoteDto = voteDto
        };
        var result = await mediator.Send(command);
        return ResolveCode(result.Exception, Ok(result.Data));
    }

    [HttpDelete("comments/{commentId:guid}/votes/current-user")]
    public async Task<IActionResult> DeleteVote(Guid commentId)
    {
        var command = new DeleteVoteCommand()
        {
            CommentId = commentId
        };
        var exception = await mediator.Send(command); 
        return ResolveCode(exception, Ok());
    }


    [HttpPost("entries/{entryId:guid}/comments")]
    [Authorize]
    public async Task<IActionResult> PostTopLevelComment(Guid entryId, PostCommentDto commentDto)
    {
        var command = new CreateTopLevelCommentCommand()
        {
            EntryId = entryId,
            CommentDto = commentDto
        };
        var result = await mediator.Send(command);
        return ResolveCode(result.Exception, CreatedAtAction(nameof(GetComments), new { entryId }, result.Data));
    }

    [HttpPost("comments/{commentId:guid}/sub-comments")]
    [Authorize]
    public async Task<IActionResult> PostSubComment(Guid commentId, PostCommentDto commentDto)
    {
        var command = new CreateSubCommentCommand()
        {
            CommentDto = commentDto,
            ParentCommentId = commentId,
        };
        var result = await mediator.Send(command);
        return ResolveCode(result.Exception, CreatedAtAction(nameof(GetSubcomments), new { commentId }, result.Data));
    }

    [HttpGet("entries/{entryId:guid}/authorized-comments")]
    [Authorize]
    public async Task<IActionResult> GetCommentsWithUsersVote(Guid entryId, int page, int size, CommentOrder order,
        OrderDirection direction)
    {
        var query = new GetCommentsAuthorizedQuery()
        {
            EntryId = entryId,
            Page = page,
            Size = size,
            Order = order,
            Direction = direction
        };
        var result = await mediator.Send(query);
        return ResolveCode(result.Exception, Ok(result.Data));
    }

    [HttpGet("entries/{entryId:guid}/comments")]
    public async Task<IActionResult> GetComments(Guid entryId, int page, int size, CommentOrder order,
        OrderDirection direction)
    {
        var query = new GetCommentsQuery()
        {
            EntryId = entryId,
            Page = page,
            Size = size,
            Order = order,
            Direction = direction
        };
        var result = await mediator.Send(query);
        return ResolveCode(result.Exception, Ok(result.Data));
    }

    [HttpGet("comments/{commentId:guid}/sub-comments")]
    public async Task<IActionResult> GetSubcomments(Guid commentId, int page, int size, CommentOrder order,
        OrderDirection direction)
    {
        var query = new GetCommentsQuery()
        {
            ParentId = commentId,
            Page = page,
            Size = size,
            Order = order,
            Direction = direction
        };
        var result = await mediator.Send(query);
        return ResolveCode(result.Exception, Ok(result.Data));
    }


    [HttpGet("comments/{commentId:guid}/authorized-sub-comments")]
    [Authorize]
    public async Task<IActionResult> GetSubcommentsAuthorized(Guid commentId, int page, int size, CommentOrder order,
        OrderDirection direction)
    {
        var query = new GetCommentsAuthorizedQuery()
        {
            ParentId = commentId,
            Page = page,
            Size = size,
            Order = order,
            Direction = direction
        };
        var result = await mediator.Send(query);
        return ResolveCode(result.Exception, Ok(result.Data));
    }
    
    [Authorize]
    [HttpDelete("/comments/{commentId:Guid}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var command = new DeleteCommentCommand(commentId);
        var exception = await mediator.Send(command);
        return OkOrError(exception);
    }
}