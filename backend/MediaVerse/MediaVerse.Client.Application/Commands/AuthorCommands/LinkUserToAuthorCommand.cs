using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.AuthorCommands;

public record LinkUserToAuthorCommand(Guid AuthorId, AddLinkedUserDto Dto ) : IRequest<BaseResponse<GetUserResponse>>;

public class LinkUserToAuthorCommandHandler(IRepository<User> userRepository,IRepository<Author> authorRepository,IMapper mapper) : IRequestHandler<LinkUserToAuthorCommand, BaseResponse<GetUserResponse>>
{
    public async Task<BaseResponse<GetUserResponse>> Handle(LinkUserToAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author is null)
        {
            return new BaseResponse<GetUserResponse>(new NotFoundException());
        }

        var user = await userRepository.GetByIdAsync(request.Dto.UserId, cancellationToken);
        if (user is null)
        {
            return new BaseResponse<GetUserResponse>(new NotFoundException());
        }

        author.User = user;
        await authorRepository.SaveChangesAsync(cancellationToken);
        var getUserResponse = mapper.Map<GetUserResponse>(user);
        return new BaseResponse<GetUserResponse>(getUserResponse);
    }
}