using MediatR;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.AmaSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediaVerse.Client.Application.Commands.AmaCommands;

public record DeleteLikeCommand(Guid AmaQuestionId) : IRequest<Exception?>;

public class DeleteLikeCommandHandler(IRepository<AmaQuestion> amaQuestionRepository, IUserService userService) : IRequestHandler<DeleteLikeCommand, Exception?>
{
    public async Task<Exception?> Handle(DeleteLikeCommand request, CancellationToken cancellationToken)
    {
        var amaQuestionSpec = new GetQuestionWithLikesSpecification(request.AmaQuestionId);
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
        amaQuestion.Users.Remove(user);
        await amaQuestionRepository.UpdateAsync(amaQuestion, cancellationToken);
        return null;
    }
}