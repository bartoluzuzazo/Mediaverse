using System.Text;
using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.EntryCommands;

public record AddBookCommand : IRequest<BaseResponse<AddBookResponse>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Release { get; set; }
    public string CoverPhoto { get; set; }
    public string Isbn { get; set; }
    public string Synopsis { get; set; }
    public List<Guid> GenreIds { get; set; }
}

public class AddBookCommandHandler(
    IRepository<Book> bookRepository,
    IRepository<Entry> entryRepository,
    IRepository<CoverPhoto> photoRepository,
    IRepository<BookGenre> bookGenreRepository)
    : IRequestHandler<AddBookCommand, BaseResponse<AddBookResponse>>
{
    public async Task<BaseResponse<AddBookResponse>> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var photo = new CoverPhoto()
        {
            Id = Guid.NewGuid(),
            Photo = Encoding.ASCII.GetBytes(request.CoverPhoto),
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
        
        var genres = request.GenreIds.Select(async id =>
        {
            var genre = await bookGenreRepository.GetByIdAsync(id, cancellationToken);
            //TODO: validate null
            return genre!;
        }).ToList();
        
        foreach (var genre in genres)
        {
            book.BookGenres.Add(await genre);
        }

        var response = new AddBookResponse()
        {
            Id = entry.Id,
            CoverPhotoId = photo.Id
        };

        await photoRepository.AddAsync(photo, cancellationToken);
        await entryRepository.AddAsync(entry, cancellationToken);
        await bookRepository.AddAsync(book, cancellationToken);
        
        return new BaseResponse<AddBookResponse>(response);
    }
}