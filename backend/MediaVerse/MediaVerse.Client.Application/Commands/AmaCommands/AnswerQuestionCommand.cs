using MediatR;
using MediaVerse.Client.Application.DTOs.AmaDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.AmaSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.AmaCommands;

public record AnswerQuestionCommand(Guid QuestionId, PutAmaAnswerDto PutAmaAnswerDto)
    : IRequest<BaseResponse<GetAnswerResponse>>;

public class AnswerQuestionCommandHandler(IRepository<AmaQuestion> amaQuestionRepository, IUserService userService)
    : IRequestHandler<AnswerQuestionCommand, BaseResponse<GetAnswerResponse>>
{
    public async Task<BaseResponse<GetAnswerResponse>> Handle(AnswerQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetAnswerResponse>(userResp.Exception);
        }

        var user = userResp.Data!;
        var questionSpec = new GetQuestionWithSessionAuthorSpecification(request.QuestionId);
        var question = await amaQuestionRepository.FirstOrDefaultAsync(questionSpec, cancellationToken);
        if (question is null)
        {
            return new BaseResponse<GetAnswerResponse>(new NotFoundException());
        }

        if (question.AmaSession.Author.UserId != user.Id)
        {
            return new BaseResponse<GetAnswerResponse>(new ForbiddenException());
        }
        
        question.Answer = request.PutAmaAnswerDto.Answer;
        await amaQuestionRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<GetAnswerResponse>(new GetAnswerResponse
        {
            Answer = question.Answer,
        });
    }
}