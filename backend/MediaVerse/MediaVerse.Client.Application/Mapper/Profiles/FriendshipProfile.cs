using AutoMapper;
using MediaVerse.Client.Application.DTOs.FriendshipDTOs;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Mapper.Profiles;

public class FriendshipProfile : Profile
{
    public FriendshipProfile()
    {
        CreateMap<Friendship, GetFriendshipResponse>();
    }
    
}