using AutoMapper;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Mapper.Profiles;

public class AuthorProfile : Profile
{
  public AuthorProfile()
  {
    CreateMap<Author, GetAuthorResponse>().ForMember(u => u.ProfilePicture,
        opt => opt.MapFrom(src => Convert.ToBase64String(src.User.ProfilePicture.Picture)));
  }
}
