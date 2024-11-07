using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.EntryQueries;

public record GetBookQuery(Guid Id) : IRequest<BaseResponse<GetBookResponse>>;

public class GetBookQueryHandler(IRepository<Book> bookRepository)
    : IRequestHandler<GetBookQuery, BaseResponse<GetBookResponse>>
{
    public async Task<BaseResponse<GetBookResponse>> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetBookByIdSpecification(request.Id);
        var book = await bookRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (book is null) return new BaseResponse<GetBookResponse>(new NotFoundException());

        var responseBook = new GetBookResponse(book.Isbn, book.Synopsis, book.BookGenres.Select(bg => bg.Name).ToList());
        return new BaseResponse<GetBookResponse>(responseBook);
    }
}