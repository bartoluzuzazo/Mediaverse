using MediatR;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.AuthorCommands;

public record CreateAuthorCommand : IRequest<BaseResponse<Guid>>
{
  public string Name { get; set; } = null!;
  public string Surname { get; set; } = null!;
  public string Bio { get; set; } = null!;
  public string ProfilePicture { get; set; } = null!;
}


public class CreateAuthorCommandHandler(
    IRepository<Author> authorRepository,
    IRepository<ProfilePicture> profilePictureRepository)
    : IRequestHandler<CreateAuthorCommand, BaseResponse<Guid>>
{
  public async Task<BaseResponse<Guid>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
  {
    byte[] photoData = Convert.FromBase64String(request.ProfilePicture);

    var profilePicture = new ProfilePicture()
    {
      Id = Guid.NewGuid(),
      Picture = photoData
    };
    await profilePictureRepository.AddAsync(profilePicture, cancellationToken);

    var author = new Author()
    {
      Id = Guid.NewGuid(),
      Name = request.Name,
      Surname = request.Surname,
      Bio = request.Bio,
      ProfilePicture = profilePicture
    };

    await authorRepository.AddAsync(author, cancellationToken);
    return new BaseResponse<Guid>(author.Id);
  }
}
