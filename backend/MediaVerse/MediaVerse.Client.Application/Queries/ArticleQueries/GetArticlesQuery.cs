using Ardalis.Specification;
using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.ArticleDtos;
using MediaVerse.Client.Application.Specifications.ArticleSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.ArticleQueries;

public class GetArticlesQuery : IRequest<BaseResponse<IEnumerable<GetArticleResponse>>>;

public class GetArticlesQueryHandler(IRepository<Article> articleRepository, IMapper mapper) : IRequestHandler<GetArticlesQuery, BaseResponse<IEnumerable<GetArticleResponse>>>
{
    public async Task<BaseResponse<IEnumerable<GetArticleResponse>>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        var articles = await articleRepository.ListAsync(new GetNewestArticlesSpecification(), cancellationToken);
        
        return new BaseResponse<IEnumerable<GetArticleResponse>>(mapper.Map<IEnumerable<GetArticleResponse>>(articles));
    }
}