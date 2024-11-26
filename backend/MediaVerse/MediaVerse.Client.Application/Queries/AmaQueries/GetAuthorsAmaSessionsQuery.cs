using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.AmaDTOs;
using MediaVerse.Client.Application.Specifications.AuthorSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.AmaQueries;

public record GetAuthorsAmaSessionsQuery(Guid AuthorId) : IRequest<BaseResponse<List<GetAmaSessionResponse>>>;

public class GetAuthorsAmaSessionsQueryHandler(IRepository<Author> authorRepository, IMapper mapper) : IRequestHandler<GetAuthorsAmaSessionsQuery, BaseResponse<List<GetAmaSessionResponse>>>
{
    public async Task<BaseResponse<List<GetAmaSessionResponse>>> Handle(GetAuthorsAmaSessionsQuery request, CancellationToken cancellationToken)
    {
        var authorSpec = new GetAuthorWithAmaSessionsSpecification(request.AuthorId);
        var author = await authorRepository.FirstOrDefaultAsync(authorSpec, cancellationToken);
        if (author is null)
        {
            return new BaseResponse<List<GetAmaSessionResponse>>(new NotFoundException());
        }

        var amaSessions = author.AmaSessions.OrderByDescending(s => s.Start);
        
        var amaSessionResponse = mapper.Map<List<GetAmaSessionResponse>>(amaSessions);
        return new BaseResponse<List<GetAmaSessionResponse>>(amaSessionResponse);
    }
}