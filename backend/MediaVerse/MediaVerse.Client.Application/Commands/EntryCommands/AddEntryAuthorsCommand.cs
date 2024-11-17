using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Client.Application.DTOs.WorkOnDTOs;
using MediaVerse.Client.Application.Specifications.AuthorRoleSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using EntryWorkOnRequest = MediaVerse.Client.Application.DTOs.WorkOnDTOs.EntryWorkOnRequest;

namespace MediaVerse.Client.Application.Commands.EntryCommands;

public record AddEntryAuthorsCommand(Guid EntryId, List<EntryWorkOnRequest>? WorkOnRequests) : IRequest<BaseResponse<EntryWorkOnResponse>>;

public class AddEntryAuthorsCommandHandler(IRepository<WorkOn> workOnRepository, IRepository<AuthorRole> roleRepository)
    : IRequestHandler<AddEntryAuthorsCommand, BaseResponse<EntryWorkOnResponse>>
{
    public async Task<BaseResponse<EntryWorkOnResponse>> Handle(AddEntryAuthorsCommand request, CancellationToken cancellationToken)
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
            EntryId = request.EntryId,
            AuthorId = r.Id,
            AuthorRoleId = roles.First(role => role.Name == r.Role).Id
        }).ToList();

        await workOnRepository.AddRangeAsync(newWorkOns, cancellationToken);

        var response = new EntryWorkOnResponse() { Id = request.EntryId};
        return new BaseResponse<EntryWorkOnResponse>(response);
    }
}