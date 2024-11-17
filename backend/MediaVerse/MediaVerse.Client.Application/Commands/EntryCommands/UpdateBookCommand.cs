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
    IRepository<Book> bookRepository,
    IRepository<BookGenre> bookGenreRepository) : IRequestHandler<UpdateBookCommand, BaseResponse<Guid>>
{
    public async Task<BaseResponse<Guid>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var bookSpecification = new GetBookByIdSpecification(request.Id);
        var book = await bookRepository.FirstOrDefaultAsync(bookSpecification, cancellationToken);
        
        book.Isbn = request.Dto.Isbn ?? book.Isbn;
        book.Synopsis = request.Dto.Synopsis ?? book.Synopsis;
        if (request.Dto.Genres is not null)
        {
            var genreSpec = new GetBookGenresByNameSpecification(request.Dto.Genres);
            var dbGenres = await bookGenreRepository.ListAsync(genreSpec, cancellationToken);
            var dbGenreNames = dbGenres.Select(g => g.Name).ToList();
            var newGenres = request.Dto.Genres.Where(genre => !dbGenreNames.Contains(genre))
                .Select(genre => new BookGenre() { Id = Guid.NewGuid(), Name = genre }).ToList();
            await bookGenreRepository.AddRangeAsync(newGenres, cancellationToken);
            dbGenres.AddRange(newGenres);
            book.BookGenres = dbGenres;
        }

        await bookRepository.SaveChangesAsync(cancellationToken);
        return new BaseResponse<Guid>(book.Id);
    }
}