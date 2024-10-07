using MediatR;
using MediaVerse.Client.Application.DTOs.RoleDTOs;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.RoleCommands;

public record AddUsersRoleCommand(Guid UserId, PostRoleDto Dto) : IRequest<BaseResponse<GetRoleStatusResponse>>;

public class AddRoleCommandHandler(IRepository<User> userRepository, IRepository<Role> roleRepository)
    : IRequestHandler<AddUsersRoleCommand, BaseResponse<GetRoleStatusResponse>>
{
    public async Task<BaseResponse<GetRoleStatusResponse>> Handle(AddUsersRoleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return new BaseResponse<GetRoleStatusResponse>(new NotFoundException());
        }

        var role = await roleRepository.GetByIdAsync(request.Dto.RoleId, cancellationToken);
        if (role is null)
        {
            return new BaseResponse<GetRoleStatusResponse>(new NotFoundException());
        }

        user.Roles.Add(role);
        await userRepository.SaveChangesAsync(cancellationToken);
        var response = new GetRoleStatusResponse()
        {
            Id = role.Id,
            IsUsers = true,
            Name = role.Name,
        };
        return new BaseResponse<GetRoleStatusResponse>(response);
    }
}