using MediatR;
using MediaVerse.Client.Application.DTOs.WorkOnDTOs;
using MediaVerse.Client.Application.Specifications.AuthorRoleSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.AuthorCommands;

public record AddAuthorEntriesCommand(Guid AuthorId, List<AuthorWorkOnRequest> WorkOnRequests) : IRequest<BaseResponse<AuthorWorkOnResponse>>;

public class AddAuthorEntriesCommandHandler(IRepository<WorkOn> workOnRepository, IRepository<AuthorRole> roleRepository) : IRequestHandler<AddAuthorEntriesCommand, BaseResponse<AuthorWorkOnResponse>>
{
    public async Task<BaseResponse<AuthorWorkOnResponse>> Handle(AddAuthorEntriesCommand request, CancellationToken cancellationToken)
    {
        var roleNames = request.WorkOnRequests.Select(r => r.Role).ToList();
        var spec = new GetAuthorRoleIdsByNameSpecification(roleNames);
        var roles = await roleRepository.ListAsync(spec, cancellationToken);
        var dbRoleNames = roles.Select(r => r.Name).ToList();
        var newRoles = roleNames.Where(roleName => !dbRoleNames.Contains(roleName))
            .Select(roleName => new AuthorRole() { Id = Guid.NewGuid(), Name = roleName }).ToList();

        await roleRepository.AddRangeAsync(newRoles, cancellationToken);
        roles.AddRange(newRoles);

        var newWorkOns = request.WorkOnRequests.Select(r => new WorkOn()
        {
            Id = Guid.NewGuid(),
            EntryId = r.EntryId,
            AuthorId = request.AuthorId,
            AuthorRoleId = roles.First(role => role.Name == r.Role).Id
        }).ToList();

        await workOnRepository.AddRangeAsync(newWorkOns, cancellationToken);

        var response = new AuthorWorkOnResponse() {  };
        return new BaseResponse<AuthorWorkOnResponse>(response);
    }
}

