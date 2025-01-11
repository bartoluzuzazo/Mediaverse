using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.ArticleDtos;
using MediaVerse.Client.Application.Specifications.ArticleSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.ArticleQueries;

public record GetArticleQuery(Guid Id) : IRequest<BaseResponse<GetArticleResponse>>;

public class GetArticleQueryHandler(IRepository<Article> articleRepository, IMapper mapper) : IRequestHandler<GetArticleQuery, BaseResponse<GetArticleResponse>>
{
    public async Task<BaseResponse<GetArticleResponse>> Handle(GetArticleQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetArticleByIdWithAuthorSpecification(request.Id);
        var article = await articleRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (article is null)
        {
            return new BaseResponse<GetArticleResponse>(new NotFoundException());
        }

        var getArticleResponse = mapper.Map<GetArticleResponse>(article);
        return new BaseResponse<GetArticleResponse>(getArticleResponse);
    }
}