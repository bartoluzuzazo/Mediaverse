using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Client.Application.Specifications.AuthorRoleSpecifications;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Client.Application.Specifications.WorkOnSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands;

public record UpdateBookCommand(Guid Id, PatchBookRequest Dto) : IRequest<BaseResponse<Guid>>;

public class UpdateBookCommandHandler(
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> coverPhotoRepository,
    IRepository<BookGenre> bookGenreRepository,
    IRepository<AuthorRole> roleRepository,
    IRepository<WorkOn> workOnRepository,
    IMapper mapper) : IRequestHandler<UpdateBookCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var specification = new GetEntryByIdSpecification(request.Id);
        var bookEntry = await entryRepository.FirstOrDefaultAsync(specification, cancellationToken);
        if (bookEntry is null)
        {
            return new BaseResponse<Guid>(new NotFoundException());
        }

        bookEntry.Description = request.Dto.Description ?? bookEntry.Description;
        bookEntry.Name = request.Dto.Name ?? bookEntry.Name;
        bookEntry.Book!.Isbn = request.Dto.Isbn ?? bookEntry.Book.Isbn;
        bookEntry.Book!.Synopsis = request.Dto.Synopsis ?? bookEntry.Book.Synopsis;
        if (request.Dto.Release is not null) bookEntry.Release = DateOnly.FromDateTime(request.Dto.Release.Value);
        if (request.Dto.Genres is not null)
        {
            var genreSpec = new GetBookGenresByNameSpecification(request.Dto.Genres);
            var dbGenres = await bookGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Dto.Genres.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new BookGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await bookGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            bookEntry.Book.BookGenres = dbGenres;
        }

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
                    opt.Items["entry"] = bookEntry;
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

        if (request.Dto.CoverPhoto is not null)
        {
            var photoData = Convert.FromBase64String(request.Dto.CoverPhoto);

            var coverPhoto = new CoverPhoto()
            {
                Id = Guid.NewGuid(),
                Photo = photoData
            };
            var newPicture = await coverPhotoRepository.AddAsync(coverPhoto, cancellationToken);
            bookEntry.CoverPhoto = newPicture;
        }

        await entryRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(bookEntry.Id);
    }
}