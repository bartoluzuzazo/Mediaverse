using MediatR;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.EntryQueries;

public record GetEntryTypeQuery(Guid Id) : IRequest<BaseResponse<string>>;

public class GetEntryTypeQueryHandler(IRepository<Entry> entryRepository) : IRequestHandler<GetEntryTypeQuery, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> Handle(GetEntryTypeQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetEntryTypeSpecification(request.Id);
        var entry = await entryRepository.SingleOrDefaultAsync(spec, cancellationToken);
        return entry is null ? new BaseResponse<string>(new NotFoundException()) : new BaseResponse<string>(entry.Type!);
    }
}