using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.FriendshipDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.FriendshipSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.FriendshipQueries;

public record GetFriendshipQuery(Guid FriendId): IRequest<BaseResponse<GetFriendshipResponse>>;

public class GetFriendshipQueryHandler(
    IRepository<Friendship> friendshipRepository,
    IMapper mapper,
    IUserService userService) : IRequestHandler<GetFriendshipQuery, BaseResponse<GetFriendshipResponse>>
{
    public async Task<BaseResponse<GetFriendshipResponse>> Handle(GetFriendshipQuery request, CancellationToken cancellationToken)
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

        var response = mapper.Map<GetFriendshipResponse>(friendship);
        return new BaseResponse<GetFriendshipResponse>(response);
    }
}