using AutoMapper;
using MediaVerse.Client.Application.DTOs.ArticleDtos;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Mapper.Profiles;

public class ArticleSummaryProfile : Profile
{

    public ArticleSummaryProfile()
    {
        CreateMap<Article, GetArticlePreviewResponse>()
            .ForMember(dest => dest.AuthorUsername, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest=>dest.AuthorPicture, opt=>opt.MapFrom(src=>src.User.ProfilePicture != null ? Convert.ToBase64String(src.User.ProfilePicture.Picture) : null));
    }
}