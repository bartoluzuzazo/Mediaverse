using MediatR;
using MediaVerse.Client.Application.Specifications.AuthorSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MediaVerse.Client.Application.Commands.AuthorCommands;

public record UpdateAuthorCommand : IRequest<BaseResponse<Guid>>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePicture { get; set; }
}

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, BaseResponse<Guid>>
{
    private readonly IRepository<Author> _authorRepository;
    private readonly IRepository<ProfilePicture> _profilePictureRepository;

    public UpdateAuthorCommandHandler(IRepository<Author> authorRepository, IRepository<ProfilePicture> profilePictureRepository)
    {
        _authorRepository = authorRepository;
        _profilePictureRepository = profilePictureRepository;
    }

    public async Task<BaseResponse<Guid>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var specification = new GetAuthorWithPhotoSpecification(request.Id);
        var author = await _authorRepository.FirstOrDefaultAsync(specification, cancellationToken);
        if (author is null)
        {
            return new BaseResponse<Guid>(new NotFoundException());
        }

        author.Bio = request.Bio ?? author.Bio;
        author.Name = request.Name ?? author.Name;
        author.Surname = request.Surname ?? author.Surname;
        if (request.ProfilePicture is not null)
        {
            var photoData = Convert.FromBase64String(request.ProfilePicture);

            var profilePicture = new ProfilePicture()
            {
                Id = Guid.NewGuid(),
                Picture = photoData
            };
           var newPicture =  await _profilePictureRepository.AddAsync(profilePicture, cancellationToken);
           author.ProfilePicture = newPicture;
        }

        var saveChangesAsync = await _authorRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(author.Id);
    }
}