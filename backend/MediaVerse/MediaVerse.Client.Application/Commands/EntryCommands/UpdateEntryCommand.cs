using AutoMapper;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Specifications.AuthorRoleSpecifications;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Client.Application.Specifications.WorkOnSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands;

public record UpdateEntryCommand(Guid Id, PatchEntryRequest Dto);

public class UpdateEntryCommandHandler(
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> coverPhotoRepository,
    IRepository<AuthorRole> roleRepository,
    IRepository<WorkOn> workOnRepository,
    IMapper mapper)
{
    public async Task<BaseResponse<Guid>> Handle(UpdateEntryCommand request, CancellationToken cancellationToken)
    {
        var entrySpecification = new GetEntryByIdSpecification(request.Id);
        var entry = await entryRepository.FirstOrDefaultAsync(entrySpecification, cancellationToken);
        if (entry is null) return new BaseResponse<Guid>(new NotFoundException());
        
        entry.Description = request.Dto.Description ?? entry.Description;
        entry.Name = request.Dto.Name ?? entry.Name;
        if (request.Dto.Release is not null) entry.Release = DateOnly.FromDateTime(request.Dto.Release.Value);
        
        if (!request.Dto.WorkOnRequests.IsNullOrEmpty())
        {
            var roleNames = request.Dto.WorkOnRequests!.Select(r => r.Role).ToList();
            var roleSpec = new GetAuthorRoleIdsByNameSpecification(roleNames);
            var roles = await roleRepository.ListAsync(roleSpec, cancellationToken);
            var dbRoleNames = roles.Select(r => r.Name).ToList();
            var newRoles = roleNames.Where(roleName => !dbRoleNames.Contains(roleName))
                .Select(roleName => new AuthorRole() { Id = Guid.NewGuid(), Name = roleName }).ToList();

            await roleRepository.AddRangeAsync(newRoles, cancellationToken);
            roles.AddRange(newRoles);

            var newWorkOns = request.Dto.WorkOnRequests!
                .Select(r => mapper.Map<WorkOn>(r, opt =>
                {
                    opt.Items["roles"] = roles;
                    opt.Items["entry"] = entry;
                }))
                .ToList();

            var woSpec = new GetWorkOnsByEntryIdSpecification(request.Id);
            var currentWorkOns = await workOnRepository.ListAsync(woSpec, cancellationToken);

            var deleted = new List<WorkOn>();
            currentWorkOns.ForEach(wo =>
            {
                var duplicate = newWorkOns.FirstOrDefault(nwo =>
                    nwo.EntryId == wo.EntryId && nwo.AuthorId == wo.AuthorId &&
                    nwo.AuthorRoleId == wo.AuthorRoleId);
                if (duplicate is not null)
                    newWorkOns.Remove(duplicate);
                else
                    deleted.Add(wo);
            });

            await workOnRepository.DeleteRangeAsync(deleted, cancellationToken);
            await workOnRepository.AddRangeAsync(newWorkOns, cancellationToken);
        }

        Console.WriteLine(request.Dto.Photo);
        if (request.Dto.Photo is not null)
        {
            var photoData = Convert.FromBase64String(request.Dto.Photo);

            var coverPhoto = new CoverPhoto()
            {
                Id = Guid.NewGuid(),
                Photo = photoData
            };
            var newPicture = await coverPhotoRepository.AddAsync(coverPhoto, cancellationToken);
            entry.CoverPhoto = newPicture;
        }

        await entryRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(entry.Id);
    }
}