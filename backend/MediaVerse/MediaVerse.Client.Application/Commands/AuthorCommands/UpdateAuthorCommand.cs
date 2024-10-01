using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.Specifications.AuthorSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MediaVerse.Client.Application.Commands.AuthorCommands;

public record UpdateAuthorCommand(Guid Id, PatchAuthorDto AuthorDto) : IRequest<BaseResponse<Guid>>;

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

        author.Bio = request.AuthorDto.Bio ?? author.Bio;
        author.Name = request.AuthorDto.Name ?? author.Name;
        author.Surname = request.AuthorDto.Surname ?? author.Surname;
        if (request.AuthorDto.ProfilePicture is not null)
        {
            var photoData = Convert.FromBase64String(request.AuthorDto.ProfilePicture);

            var profilePicture = new ProfilePicture()
            {
                Id = Guid.NewGuid(),
                Picture = photoData
            };
            var newPicture = await profilePictureRepository.AddAsync(profilePicture, cancellationToken);
            author.ProfilePicture = newPicture;
        }

        var saveChangesAsync = await authorRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(author.Id);
    }
}