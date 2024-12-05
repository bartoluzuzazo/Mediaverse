using AutoMapper;
using MediaVerse.Client.Application.DTOs.AmaDTOs;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Mapper.Profiles;

public class AmaSessionProfile : Profile
{
    public AmaSessionProfile()
    {
        CreateMap<AmaSession, GetAmaSessionResponse>()
            .ForMember(s => s.AuthorName, opt => opt.MapFrom(s => s.Author.Name))
            .ForMember(s => s.AuthorSurname, opt => opt.MapFrom(s => s.Author.Surname))
            .ForMember(s => s.ProfilePicture,
                opt => opt.MapFrom(s => Convert.ToBase64String(s.Author.ProfilePicture.Picture)))
            .ForMember(s => s.AuthorUserId, opt => opt.MapFrom(s => s.Author.UserId))
            .ForMember(s => s.AuthorId, opt => opt.MapFrom(s => s.Author.Id))
            .ForMember(s => s.Status, opt => opt.MapFrom(s =>
                s.End < s.Start ? AmaStatus.Cancelled
                : DateTime.UtcNow < s.Start ? AmaStatus.Upcoming
                : DateTime.UtcNow < s.End ? AmaStatus.Active
                : AmaStatus.Finished
            ));
    }
}