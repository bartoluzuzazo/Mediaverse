using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.DTOs.WorkOnDTOs;
using MediaVerse.Client.Application.Specifications.AuthorRoleSpecifications;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands;

public record UpdateBookCommand : IRequest<BaseResponse<Guid>>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? Release { get; set; }
    public string? CoverPhoto { get; set; }
    public string? Isbn { get; set; }
    public string? Synopsis { get; set; }
    public List<string>? Genres { get; set; }
    public List<EntryWorkOnRequest>? WorkOnRequests { get; set; }
}

public class UpdateBookCommandHandler(
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> coverPhotoRepository,
    IRepository<BookGenre> bookGenreRepository,
    IRepository<AuthorRole> roleRepository,
    IRepository<WorkOn> workOnRepository) : IRequestHandler<UpdateBookCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var specification = new GetBookByIdSpecification(request.Id);
        var book = await entryRepository.FirstOrDefaultAsync(specification, cancellationToken);
        if (book is null)
        {
            return new BaseResponse<Guid>(new NotFoundException());
        }

        book.Description = request.Description ?? book.Description;
        book.Name = request.Name ?? book.Name;
        book.Book!.Isbn = request.Isbn ?? book.Book.Isbn;
        book.Book!.Synopsis = request.Synopsis ?? book.Book.Synopsis;
        if (request.Release is not null) book.Release = DateOnly.FromDateTime(request.Release.Value);
        if (request.Genres is not null)
        {
            var genreSpec = new GetBookGenresByNameSpecification(request.Genres);
            var dbGenres = await bookGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Genres.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new BookGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await bookGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            book.Book.BookGenres = dbGenres;
        }

        //TODO: workOns

        if (request.CoverPhoto is not null)
        {
            var photoData = Convert.FromBase64String(request.CoverPhoto);

            var coverPhoto = new CoverPhoto()
            {
                Id = Guid.NewGuid(),
                Photo = photoData
            };
            var newPicture = await coverPhotoRepository.AddAsync(coverPhoto, cancellationToken);
            book.CoverPhoto = newPicture;
        }

        await entryRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(book.Id);
    }
}