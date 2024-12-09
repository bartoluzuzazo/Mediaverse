using AutoMapper;
using MediaVerse.Client.Application.DTOs.ReviewDtos;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Mapper.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, GetReviewResponse>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.ProfilePicture, opt =>
                opt.MapFrom(src =>
                    src.User.ProfilePicture != null ? Convert.ToBase64String(src.User.ProfilePicture.Picture) : null));
    }
}