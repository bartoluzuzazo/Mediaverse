using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Specifications.AuthorRoleSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands;

public record AddEntryCommand(
    string Name,
    string Description,
    DateTime Release,
    string CoverPhoto,
    List<IEntryWorkOnRequest>? WorkOnRequests) : IRequest<BaseResponse<AddEntryResponse>>;

public class AddEntryCommandHandler(
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<WorkOn> workOnRepository,
    IRepository<AuthorRole> roleRepository,
    IMapper mapper) 
    : IRequestHandler<AddEntryCommand, BaseResponse<AddEntryResponse>>
{
    public async Task<BaseResponse<AddEntryResponse>> Handle(AddEntryCommand request, CancellationToken cancellationToken)
    {
        var photo = new CoverPhoto()
        {
            Id = Guid.NewGuid(),
            Photo = Convert.FromBase64String(request.CoverPhoto),
        };

        var entry = new Entry()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Release = DateOnly.FromDateTime(request.Release),
            CoverPhotoId = photo.Id
        };
        
        await photoRepository.AddAsync(photo, cancellationToken);
        await entryRepository.AddAsync(entry, cancellationToken);
        
        if (!request.WorkOnRequests.IsNullOrEmpty())
        {
            var roleNames = request.WorkOnRequests!.Select(r => r.Role).ToList();
            var spec = new GetAuthorRoleIdsByNameSpecification(roleNames);
            var roles = await roleRepository.ListAsync(spec, cancellationToken);
            var dbRoleNames = roles.Select(r => r.Name).ToList();
            var newRoles = roleNames.Where(roleName => !dbRoleNames.Contains(roleName))
                .Select(roleName => new AuthorRole() { Id = Guid.NewGuid(), Name = roleName }).ToList();

            await roleRepository.AddRangeAsync(newRoles, cancellationToken);
            roles.AddRange(newRoles);

            var newWorkOns = request.WorkOnRequests!
                .Select(r => mapper.Map<WorkOn>(r, opt =>
                {
                    opt.Items["roles"] = roles;
                    opt.Items["entry"] = entry;
                }))
                .ToList();
            await workOnRepository.AddRangeAsync(newWorkOns, cancellationToken);
        }

        var response = new AddEntryResponse(entry.Id, photo.Id);
        return new BaseResponse<AddEntryResponse>(response);
    }
}