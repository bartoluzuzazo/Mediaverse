using MediatR;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.RoleCommands;

public record DeleteUsersRoleCommand(Guid UserId, Guid RoleId) : IRequest<Exception?>;

public class DeleteUsersRoleCommandHandler(IRepository<User> userRepository) : IRequestHandler<DeleteUsersRoleCommand, Exception?>
{
    public async Task<Exception?> Handle(DeleteUsersRoleCommand request, CancellationToken cancellationToken)
    {
        var getUserWithRolesSpecification = new GetUserWithRolesSpecification(request.UserId);
        var user = await userRepository.FirstOrDefaultAsync(getUserWithRolesSpecification, cancellationToken);
        if (user is null)
        {
            return new NotFoundException();
        }

        var role = user.Roles.FirstOrDefault(r => r.Id == request.RoleId);
        if (role is not null)
        {
            user.Roles.Remove(role);
        }
        await userRepository.SaveChangesAsync(cancellationToken);
        return null;
    }
}