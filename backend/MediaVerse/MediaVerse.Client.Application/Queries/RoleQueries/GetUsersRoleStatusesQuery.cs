using MediatR;
using MediaVerse.Client.Application.DTOs.RoleDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.RoleSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.RoleQueries;

public record GetUsersRoleStatusesQuery(Guid UserId) : IRequest<BaseResponse<List<GetRoleStatusResponse>>>;

public class GetUsersRoleStatusesQueryHandler(IRepository<Role> roleRepository,IUserService userService) : IRequestHandler<GetUsersRoleStatusesQuery, BaseResponse<List<GetRoleStatusResponse>>>
{
    public async Task<BaseResponse<List<GetRoleStatusResponse>>> Handle(GetUsersRoleStatusesQuery request, CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<List<GetRoleStatusResponse>>(userResp.Exception);
        }
        var user = userResp.Data!;

        var getRoleStatusesSpecification = new GetRoleStatusesSpecification(request.UserId);
        var roleStatuses = await roleRepository.ListAsync(getRoleStatusesSpecification, cancellationToken);
        return new BaseResponse<List<GetRoleStatusResponse>>(roleStatuses);
    }
}