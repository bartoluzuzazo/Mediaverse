using MediatR;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MediaVerse.Client.Application.Commands.AuthorCommands;

public record CreateAuthorCommand : IRequest<BaseResponse<Guid>>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Bio { get; set; }
    public string ProfilePicture { get; set; }
}


public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, BaseResponse<Guid>>
{
    private readonly IRepository<Author> _authorRepository;
    private readonly IRepository<ProfilePicture> _profilePictureRepository;

    public CreateAuthorCommandHandler(IRepository<Author> authorRepository, IRepository<ProfilePicture> profilePictureRepository)
    {
        _authorRepository = authorRepository;
        _profilePictureRepository = profilePictureRepository;
    }

    public async Task<BaseResponse<Guid>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        byte[] photoData = Convert.FromBase64String(request.ProfilePicture);

        var profilePicture = new ProfilePicture()
        {
            Id = Guid.NewGuid(),
            Picture = photoData
        };
        await _profilePictureRepository.AddAsync(profilePicture, cancellationToken);

        var author = new Author()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            Bio = request.Bio,
            ProfilePicture = profilePicture
        };

        await _authorRepository.AddAsync(author, cancellationToken);
        return new BaseResponse<Guid>(author.Id);
    }
}