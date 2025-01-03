using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.ArticleDtos;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.ArticleCommands;

public record UpdateArticleCommand(Guid Id, UpdateArticleDto dto) : IRequest<BaseResponse<GetArticleResponse>>;

public class UpdateArticleCommandHandler(IRepository<Article> articleRepository, IUserService userService, IMapper mapper) : IRequestHandler<UpdateArticleCommand, BaseResponse<GetArticleResponse>>
{
    public async Task<BaseResponse<GetArticleResponse>> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetArticleResponse>(userResp.Exception);
        }

        var user = userResp.Data!;

        var article = await articleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (article is null)
        {
            return new BaseResponse<GetArticleResponse>(new NotFoundException("Article not found"));
        }

        if (article.UserId != user.Id)
        {
            return new BaseResponse<GetArticleResponse>(new ForbiddenException("You are not authorized to update this article"));
        }
        
        article.Title = request.dto.Title;
        article.Content = request.dto.Content;
        article.Lede = request.dto.Lede;
        await articleRepository.SaveChangesAsync(cancellationToken);
        var getArticleResponse = mapper.Map<GetArticleResponse>(article);
        return new BaseResponse<GetArticleResponse>(getArticleResponse);
    }
}