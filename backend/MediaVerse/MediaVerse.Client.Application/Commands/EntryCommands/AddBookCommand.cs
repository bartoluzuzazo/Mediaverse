using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.DTOs.WorkOnDTOs;
using MediaVerse.Client.Application.Specifications.AuthorRoleSpecifications;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands;

public record AddBookCommand : IRequest<BaseResponse<AddEntryResponse>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Release { get; set; }
    public string CoverPhoto { get; set; }
    public string Isbn { get; set; }
    public string Synopsis { get; set; }
    public List<string>? Genres { get; set; }
    public List<EntryWorkOnRequest>? WorkOnRequests { get; set; }
}

public class AddBookCommandHandler(
    IRepository<Book> bookRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<BookGenre> bookGenreRepository,
    IRepository<WorkOn> workOnRepository, 
    IRepository<AuthorRole> roleRepository)
    : IRequestHandler<AddBookCommand, BaseResponse<AddEntryResponse>>
{
    public async Task<BaseResponse<AddEntryResponse>> Handle(AddBookCommand request,
        CancellationToken cancellationToken)
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

        var book = new Book()
        {
            Id = entry.Id,
            Isbn = request.Isbn,
            Synopsis = request.Synopsis,
            BookGenres = new List<BookGenre>()
        };

        if (!request.Genres.IsNullOrEmpty())
        {
            var genreSpec = new GetBookGenresByNameSpecification(request.Genres!);
            var dbGenres = await bookGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Genres!.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new BookGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();

            await bookGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);

            book.BookGenres = dbGenres;
        }

        await photoRepository.AddAsync(photo, cancellationToken);
        await entryRepository.AddAsync(entry, cancellationToken);
        await bookRepository.AddAsync(book, cancellationToken);
        
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

            var newWorkOns = request.WorkOnRequests!.Select(r => new WorkOn()
            {
                Id = Guid.NewGuid(),
                EntryId = book.Id,
                AuthorId = r.Id,
                AuthorRoleId = roles.First(role => role.Name == r.Role).Id
            }).ToList();
            await workOnRepository.AddRangeAsync(newWorkOns, cancellationToken);
        }

        var response = new AddEntryResponse()
        {
            Id = entry.Id,
            CoverPhotoId = photo.Id
        };
        
        return new BaseResponse<AddEntryResponse>(response);
    }
}