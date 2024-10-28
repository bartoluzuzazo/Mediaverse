using MediatR;
using MediaVerse.Client.Application.DTOs.AmaDTOs;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.AmaSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Queries.AmaQueries;

public record GetAmaQuestionsAuthorizedQuery(Guid SessionId, int Page, int Size, QuestionOrder Order, OrderDirection Direction, QuestionStatus Status ) : IRequest<BaseResponse<Page<GetAmaQuestionResponse>>>;

public class GetAmaQuestionsAuthorizedQueryHandler(IRepository<AmaQuestion> amaQuestionRepository, IRepository<AmaSession> amaSessionRepository, IUserAccessor userAccessor) : IRequestHandler<GetAmaQuestionsAuthorizedQuery, BaseResponse<Page<GetAmaQuestionResponse>>>
{
    public async Task<BaseResponse<Page<GetAmaQuestionResponse>>> Handle(GetAmaQuestionsAuthorizedQuery request,
        CancellationToken cancellationToken)
    {
        var id = userAccessor.Id;
        if (id is null)
        {
            return new BaseResponse<Page<GetAmaQuestionResponse>>(new ProblemException());
        }
        var amaSession = await amaSessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (amaSession is null)
        {
            return new BaseResponse<Page<GetAmaQuestionResponse>>(new NotFoundException());
        }

        var amaQuestionSpecification = new GetQuestionsSpecification(request.SessionId, id, request.Page,
            request.Size, request.Order, request.Direction, request.Status);
        var questions = await amaQuestionRepository.ListAsync(amaQuestionSpecification, cancellationToken);
        var questionCount = await amaQuestionRepository.CountAsync(amaQuestionSpecification, cancellationToken);
        var response = new Page<GetAmaQuestionResponse>(questions, request.Page, questionCount, request.Size);
        return new BaseResponse<Page<GetAmaQuestionResponse>>(response);

    }
}