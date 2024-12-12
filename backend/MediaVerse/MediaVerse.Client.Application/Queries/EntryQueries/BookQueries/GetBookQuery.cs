using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.BookDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications.BookSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.EntryQueries.BookQueries;

public record GetBookQuery(Guid Id) : IRequest<BaseResponse<GetBookResponse>>;

public class GetBookQueryHandler(IRepository<Book> bookRepository, IRepository<Entry> entryRepository)
    : GetBaseEntryQueryHandler(entryRepository), IRequestHandler<GetBookQuery, BaseResponse<GetBookResponse>>
{
    public async Task<BaseResponse<GetBookResponse>> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var query = new GetBaseEntryQuery(request.Id);
        var response = await base.Handle(query, cancellationToken);
        if (response.Exception is not null) return new BaseResponse<GetBookResponse>(response.Exception);
        
        var spec = new GetBookByIdSpecification(request.Id);
        var book = await bookRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (book is null) return new BaseResponse<GetBookResponse>(new NotFoundException());

        var responseBook = new GetBookResponse(book.Isbn, book.Synopsis, book.BookGenres.Select(bg => bg.Name).ToList(), response.Data);
        return new BaseResponse<GetBookResponse>(responseBook);
    }
}