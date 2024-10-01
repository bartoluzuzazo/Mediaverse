using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.FriendshipDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.FriendshipSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.FriendshipCommands;

public record CreateFriendshipCommand(Guid UserId) : IRequest<BaseResponse<GetFriendshipResponse>>;

public class CreateFriendshipCommandHandler(
    IRepository<Friendship> friendshipRepository,
    IUserService userService,
    IRepository<User> userRepository,
    IMapper mapper) : IRequestHandler<CreateFriendshipCommand, BaseResponse<GetFriendshipResponse>>
{
    public async Task<BaseResponse<GetFriendshipResponse>> Handle(CreateFriendshipCommand request,
        CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetFriendshipResponse>(userResp.Exception);
        }

        var currentUser = userResp.Data!;

        var newFriend = await userRepository.GetByIdAsync(request.UserId);
        if (newFriend is null)
        {
            return new BaseResponse<GetFriendshipResponse>(new NotFoundException());
        }

        var conflictSpec = new GetFriendshipSpecification(currentUser.Id, request.UserId);
        var existsConflict = await friendshipRepository.AnyAsync(conflictSpec, cancellationToken);
        if (existsConflict)
        {
            return new BaseResponse<GetFriendshipResponse>(new ConflictException("Friendship request already sent"));
        }

        var friendship = new Friendship()
        {
            Approved = false,
            User = currentUser,
            User2 = newFriend,
        };
        var saved = await friendshipRepository.AddAsync(friendship, cancellationToken);

        var response = mapper.Map<GetFriendshipResponse>(saved);
        return new BaseResponse<GetFriendshipResponse>(response);
    }
}