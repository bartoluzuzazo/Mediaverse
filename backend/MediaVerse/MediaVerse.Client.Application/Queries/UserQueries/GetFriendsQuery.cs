using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Specifications.FriendshipSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.UserQueries;

public record GetFriendsQuery(Guid UserId): IRequest<BaseResponse<List<GetUserResponse>>>;

public class GetFriendsQueryHandler(IRepository<User> userRepository, IRepository<Friendship> friendshipRepository, IMapper mapper): IRequestHandler<GetFriendsQuery, BaseResponse<List<GetUserResponse>>>
{
    public async Task<BaseResponse<List<GetUserResponse>>> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
    {
        var user = await  userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return new BaseResponse<List<GetUserResponse>>(new NotFoundException());
        }

        var spec = new GetFriendsSpecification(user.Id);
        var friends = await friendshipRepository.ListAsync(spec, cancellationToken);
        return new BaseResponse<List<GetUserResponse>>(friends);
    }
}