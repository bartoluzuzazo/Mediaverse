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

public record ApproveFriendshipCommand(Guid FriendId) : IRequest<BaseResponse<GetFriendshipResponse>>;

public class
    ApproveFriendshipCommandHandler(
        IUserService userService,
        IRepository<Friendship> friendshipRepository,
        IMapper mapper) : IRequestHandler<ApproveFriendshipCommand, BaseResponse<GetFriendshipResponse>>
{
    public async Task<BaseResponse<GetFriendshipResponse>> Handle(ApproveFriendshipCommand request,
        CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetFriendshipResponse>(userResp.Exception);
        }

        var currentUser = userResp.Data!;
        var spec = new GetFriendshipSpecification(currentUser.Id, request.FriendId);
        var friendship = await friendshipRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (friendship is null)
        {
            return new BaseResponse<GetFriendshipResponse>(new NotFoundException());
        }

        if (friendship.User2Id != currentUser.Id)
        {
            return new BaseResponse<GetFriendshipResponse>(
                new ConflictException("User is not the recipient of the invitation"));
        }

        friendship.Approved = true;

        await friendshipRepository.SaveChangesAsync(cancellationToken);

        var friendshipResponse = mapper.Map<GetFriendshipResponse>(friendship);
        return new BaseResponse<GetFriendshipResponse>(friendshipResponse);
    }
}