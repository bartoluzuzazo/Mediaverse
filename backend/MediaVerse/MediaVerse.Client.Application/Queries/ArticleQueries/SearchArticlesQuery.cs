using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.ArticleDtos;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.Specifications.ArticleSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.ArticleQueries;

public record SearchArticlesQuery(string Searched, int Page, int Size) : IRequest<BaseResponse<Page<GetArticlePreviewResponse>>>;

public class SearchArticlesQueryHandler(IRepository<Article> articleRepository, IMapper mapper) : IRequestHandler<SearchArticlesQuery, BaseResponse<Page<GetArticlePreviewResponse>>>
{
    public async Task<BaseResponse<Page<GetArticlePreviewResponse>>> Handle(SearchArticlesQuery request, CancellationToken cancellationToken)
    {
        var specification = new GetArticleFullTextSearchNoTrackingSpecification(request.Searched, request.Page, request.Size);
        var articles = await articleRepository.ListAsync(specification, cancellationToken);
        var count = await articleRepository.CountAsync(specification, cancellationToken);

        var articlePreviews = mapper.Map<List<GetArticlePreviewResponse>>(articles);
        var page = new Page<GetArticlePreviewResponse>(articlePreviews, request.Page, count,request.Size );
        return new BaseResponse<Page<GetArticlePreviewResponse>>(page);
    }
}