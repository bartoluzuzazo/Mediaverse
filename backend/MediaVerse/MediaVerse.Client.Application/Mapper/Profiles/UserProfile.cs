using AutoMapper;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Mapper.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, GetUserResponse>().ForMember(u => u.ProfilePicture,
            opt => opt.MapFrom(u => Convert.ToBase64String(u.ProfilePicture.Picture)))
            .ForMember(u => u.Authors, opt => opt.Ignore());
    }
    
}