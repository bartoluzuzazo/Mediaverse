using AutoMapper;
using MediaVerse.Client.Application.DTOs.WorkOnDTOs;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Mapper.Profiles;

public class EntryProfile : Profile
{
    public EntryProfile()
    {
        CreateMap<EntryWorkOnRequest, WorkOn>()
            .ForMember(wo => wo.Id, opt => opt.MapFrom(_=>Guid.NewGuid()))
            .ForMember(wo => wo.AuthorId, opt => opt.MapFrom(r=>r.Id))
            .ForMember(wo => wo.EntryId, opt => opt.MapFrom((src, dst, _, ctx) =>
            {
                var entry = ctx.Items["entry"] as Entry;
                return entry?.Id;
            }))
            .ForMember(wo => wo.AuthorRoleId, opt => opt.MapFrom((src, dst, _, ctx) =>
            {
                var roles = ctx.Items["roles"] as List<AuthorRole>;
                return roles?.First(role => role.Name == src.Role).Id;
            }));
    }
}