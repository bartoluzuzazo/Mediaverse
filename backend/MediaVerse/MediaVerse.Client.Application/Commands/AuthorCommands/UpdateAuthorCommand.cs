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

public class UpdateAuthorCommandHandler(
    IRepository<Author> authorRepository,
    IRepository<ProfilePicture> profilePictureRepository)
    : IRequestHandler<UpdateAuthorCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var specification = new GetAuthorWithPhotoSpecification(request.Id);
        var author = await authorRepository.FirstOrDefaultAsync(specification, cancellationToken);
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
           var newPicture =  await profilePictureRepository.AddAsync(profilePicture, cancellationToken);
           author.ProfilePicture = newPicture;
        }

        var saveChangesAsync = await authorRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(author.Id);
    }
}