using MediatR;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Commands.EntryCommands;

public record AddBookCommand(Guid EntryId, string Isbn, string Synopsis, List<string>? Genres) : IRequest<BaseResponse<Guid>>;

public class AddBookCommandHandler(
    IRepository<Book> bookRepository,
    IRepository<BookGenre> bookGenreRepository)
    : IRequestHandler<AddBookCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(AddBookCommand request,
        CancellationToken cancellationToken)
    {
        var book = new Book()
        {
            Id = request.EntryId,
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
        
        await bookRepository.AddAsync(book, cancellationToken);

        return new BaseResponse<Guid>(book.Id);
    }
}