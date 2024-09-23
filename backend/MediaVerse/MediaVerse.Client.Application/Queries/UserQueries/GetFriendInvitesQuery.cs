using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.FriendshipSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.UserQueries;

public record GetFriendInvitesQuery() : IRequest<BaseResponse<List<GetUserResponse>>>;

public class GetFriendInvitesQueryHandler(IRepository<Friendship> friendshipRepository, IUserService userService, IMapper mapper)
    : IRequestHandler<GetFriendInvitesQuery, BaseResponse<List<GetUserResponse>>>
{
    public async Task<BaseResponse<List<GetUserResponse>>> Handle(GetFriendInvitesQuery request, CancellationToken cancellationToken)
    {
        var userResponse = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResponse.Exception is not null)
        {
            return new BaseResponse<List<GetUserResponse>>(userResponse.Exception);
        }


        var spec = new GetFriendshipInvitesSpecification(userResponse.Data!.Id);
        var invites = await friendshipRepository.ListAsync(spec, cancellationToken);
        var invitingUsers = invites.Select(friendship => friendship.User);
        var response = mapper.Map<List<GetUserResponse>>(invitingUsers);
        return new BaseResponse<List<GetUserResponse>>(response);
    }
}