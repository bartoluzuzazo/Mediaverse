using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.Specifications.AuthorSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.AuthorQueries;

public record SearchAuthorsQuery(int Page, int Size, string Query): IRequest<BaseResponse<Page<GetAuthorResponse>>>;

public class SearchAuthorsQueryHandler(IMapper mapper,IRepository<Author> authorRepository) : IRequestHandler<SearchAuthorsQuery, BaseResponse<Page<GetAuthorResponse>>>
{
    public async Task<BaseResponse<Page<GetAuthorResponse>>> Handle(SearchAuthorsQuery request, CancellationToken cancellationToken)
    {
        var spec = new SearchAuthorSpecification(request.Query,request.Page, request.Size);
        var authors  = await authorRepository.ListAsync(spec, cancellationToken);
        var authorCount = await authorRepository.CountAsync(spec, cancellationToken);
        var response = mapper.Map<List<GetAuthorResponse>>(authors);
        
        var page = new Page<GetAuthorResponse>(response, request.Page, authorCount,request.Size);
        return new BaseResponse<Page<GetAuthorResponse>>(page);
        
    }
}