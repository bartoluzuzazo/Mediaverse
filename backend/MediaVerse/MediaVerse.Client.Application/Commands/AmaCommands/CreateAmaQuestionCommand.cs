using MediatR;
using MediaVerse.Client.Application.DTOs.AmaDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.AmaCommands;

public record CreateAmaQuestionCommand(Guid SessionId, PostAmaQuestionDto PostAmaQuestionDto)
    : IRequest<BaseResponse<GetAmaQuestionResponse>>;

public class CreateAmaQuestionCommandHandler(
    IUserAccessor userAccessor,
    IRepository<User> userRepository,
    IRepository<AmaQuestion> amaQuestionRepository,
    IRepository<AmaSession> amaSessionRepository)
    : IRequestHandler<CreateAmaQuestionCommand, BaseResponse<GetAmaQuestionResponse>>
{
  public async Task<BaseResponse<GetAmaQuestionResponse>> Handle(CreateAmaQuestionCommand request,
      CancellationToken cancellationToken)
  {
    var amaSession = await amaSessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
    if (amaSession is null)
    {
      return new BaseResponse<GetAmaQuestionResponse>(new NotFoundException());
    }

    if (!(amaSession.Start <= DateTime.UtcNow && DateTime.UtcNow <= amaSession.End))
    {
      return new BaseResponse<GetAmaQuestionResponse>(new ConflictException("Can only create questions when session is active"));
    }


    var email = userAccessor.Email;
    if (email is null)
    {
      return new BaseResponse<GetAmaQuestionResponse>(new ProblemException());
    }

    var userSpec = new GetUserByEmailWithPictureSpecification(email);


    var user = await userRepository.FirstOrDefaultAsync(userSpec, cancellationToken);
    if (user is null)
    {
      return new BaseResponse<GetAmaQuestionResponse>(new NotFoundException());
    }

    var question = new AmaQuestion
    {
      Id = Guid.NewGuid(),
      User = user,
      Content = request.PostAmaQuestionDto.Content,
      Users = new List<User> { user },
      AmaSession = amaSession,
      CreatedAt = DateTime.UtcNow
    };
    await amaQuestionRepository.AddAsync(question, cancellationToken);
    var response = new GetAmaQuestionResponse
    {
      Id = question.Id,
      AmaSessionId = question.AmaSessionId,
      Username = user.Username,
      ProfilePicture = Convert.ToBase64String(user.ProfilePicture.Picture),
      UserId = user.Id,
      Likes = question.Users.Count,
      Content = question.Content,
      LikedByUser = question.Users.Any(u => u.Id == user.Id),
    };
    return new BaseResponse<GetAmaQuestionResponse>(response);
  }
}
