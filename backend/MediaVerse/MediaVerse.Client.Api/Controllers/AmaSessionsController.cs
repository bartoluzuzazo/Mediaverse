using MediatR;
using MediaVerse.Client.Api.Filters;
using MediaVerse.Client.Application.Commands.AmaCommands;
using MediaVerse.Client.Application.DTOs.AmaDTOs;
using MediaVerse.Client.Application.Queries.AmaQueries;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AmaSessionsController(IMediator mediator) : BaseController
{
  [HttpPost]
  [Authorize]
  [ValidateModel]
  public async Task<IActionResult> PostAmaSession(CreateAmaSessionCommand command)
  {
    var response = await mediator.Send(command);
    return ResolveCode(response.Exception, CreatedAtAction(nameof(GetAmaSession), new { sessionId = response.Data?.Id }, response.Data));
  }

  [HttpGet("{sessionId:guid}")]
  public async Task<IActionResult> GetAmaSession(Guid sessionId)
  {
    var query = new GetAmaSessionQuery(sessionId);
    var response = await mediator.Send(query);
    return OkOrError(response);
  }

  [HttpPut("{sessionId:guid}/ending")]
  [Authorize]
  public async Task<IActionResult> PostSessionEnding(EndAmaSessionCommand command)
  {
    var response = await mediator.Send(command);
    return OkOrError(response);
  }

  [HttpPost("{sessionId:guid}/questions")]
  [Authorize]
  public async Task<IActionResult> PostQuestion(Guid sessionId, PostAmaQuestionDto postAmaQuestionDto)
  {
    var command = new CreateAmaQuestionCommand(sessionId, postAmaQuestionDto);
    var response = await mediator.Send(command);
    return ResolveCode(response.Exception, CreatedAtAction(nameof(GetQuestions), new { sessionId = sessionId }, response.Data));
  }

  [HttpGet("{sessionId:guid}/questions")]
  public async Task<IActionResult> GetQuestions(Guid sessionId, int page, int size, QuestionOrder order, OrderDirection direction, QuestionStatus status)
  {
    var query = new GetAmaQuestionsQuery(sessionId, page, size, order, direction, status);
    var response = await mediator.Send(query);
    return OkOrError(response);
  }

  [HttpGet("{sessionId:guid}/authorized-questions")]
  [Authorize]
  public async Task<IActionResult> GetAuthorizedQuestions(Guid sessionId, int page, int size, QuestionOrder order, OrderDirection direction, QuestionStatus status)
  {
    var query = new GetAmaQuestionsAuthorizedQuery(sessionId, page, size, order, direction, status);
    var response = await mediator.Send(query);
    return OkOrError(response);

  }

  [HttpPut("questions/{questionId:guid}/answer")]
  [Authorize]
  public async Task<IActionResult> PutAmaAnswer(Guid questionId, PutAmaAnswerDto putAmaAnswerDto)
  {
    var command = new AnswerQuestionCommand(questionId, putAmaAnswerDto);
    var response = await mediator.Send(command);
    return OkOrError(response);
  }

  [HttpPut("questions/{questionId:guid}/like")]
  [Authorize]
  public async Task<IActionResult> PutLike(Guid questionId)
  {
    var command = new LikeQuestionCommand(questionId);
    var response = await mediator.Send(command);
    return OkOrError(response);
  }

  [HttpDelete("questions/{questionId:guid}/like")]
  [Authorize]
  public async Task<IActionResult> DeleteLike(Guid questionId)
  {
    var command = new DeleteLikeCommand(questionId);
    var response = await mediator.Send(command);
    return OkOrError(response);
  }
}
