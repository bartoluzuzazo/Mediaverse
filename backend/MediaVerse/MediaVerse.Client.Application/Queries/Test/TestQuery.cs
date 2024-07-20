using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs;
using MediaVerse.Domain.AggregatesModel;

namespace MediaVerse.Client.Application.Queries.Test;

public class TestQuery : IRequest<TestDto>
{
    
}

public class TestQueryHandler : IRequestHandler<TestQuery, TestDto>
{
    private readonly IMapper _mapper;

    public TestQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<TestDto> Handle(TestQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<TestDto>(new TestEntity() {FirstName = "Test"});
    }
}