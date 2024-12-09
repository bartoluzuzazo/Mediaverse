using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.ArticleDtos;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.ArticleCommands;

public record CreateArticleCommand : IRequest<BaseResponse<GetArticleResponse>>
{
    public string Title { get; set; }

    public string Content { get; set; }
    public string Lede { get; set; }
}

public class CreateArticleCommandHandler(IUserService userService, IRepository<Article> articleRepository, IMapper mapper) : IRequestHandler<CreateArticleCommand, BaseResponse<GetArticleResponse>>
{
    public async Task<BaseResponse<GetArticleResponse>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserWithPictureAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetArticleResponse>(userResp.Exception);
        }

        var user = userResp.Data!;
        var article = new Article
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            Lede = request.Lede,
            User = user,
            Timestamp = DateTime.Now
        };
        articleRepository.AddAsync(article, cancellationToken);
        var getArticleResponse = mapper.Map<GetArticleResponse>(article);
        return new BaseResponse<GetArticleResponse>(getArticleResponse);
    }
}