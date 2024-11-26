using MediatR;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.AmaSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.AmaCommands;

public record LikeQuestionCommand(Guid QuestionId) : IRequest<Exception?>;

public class LikeQuestionCommandHandler(IRepository<AmaQuestion> amaQuestionRepository, IUserService userService)
    : IRequestHandler<LikeQuestionCommand, Exception?>
{
    public async Task<Exception?> Handle(LikeQuestionCommand request, CancellationToken cancellationToken)
    {
        var amaQuestionSpec = new GetQuestionWithLikesSpecification(request.QuestionId);
        var amaQuestion = await amaQuestionRepository.FirstOrDefaultAsync(amaQuestionSpec, cancellationToken);
        if (amaQuestion is null)
        {
            return new NotFoundException();
        }

        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return userResp.Exception;
        }

        var user = userResp.Data!;
        amaQuestion.Users.Add(user);
        await amaQuestionRepository.SaveChangesAsync(cancellationToken);
        return null;
    }
}