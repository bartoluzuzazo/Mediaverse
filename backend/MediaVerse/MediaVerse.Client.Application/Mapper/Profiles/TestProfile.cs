using AutoMapper;
using MediaVerse.Client.Application.DTOs;
using MediaVerse.Domain.AggregatesModel;

namespace MediaVerse.Client.Application.Mapper.Profiles;

public class TestProfile : Profile
{
    public TestProfile()
    {
        CreateMap<TestEntity, TestDto>();
    }
}