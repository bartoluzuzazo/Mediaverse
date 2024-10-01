using MediatR;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.DTOs.EntryDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Queries.EntryQueries;

public record GetRatedEntriesQuery(Guid UserId, int Page, int Size, RatedEntryOrder EntryOrder, OrderDirection Direction)
    : IRequest<BaseResponse<Page<GetRatedEntryResponse>>>;

public class GetRatedEntriesQueryHandler(IRepository<User> userRepository, IRepository<Entry> entryRepository)
    : IRequestHandler<GetRatedEntriesQuery, BaseResponse<Page<GetRatedEntryResponse>>>
{
    public async Task<BaseResponse<Page<GetRatedEntryResponse>>> Handle(GetRatedEntriesQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return new BaseResponse<Page<GetRatedEntryResponse>>(new NotFoundException());
        }

        var spec = new GetRatedEntriesSpecification(user.Id, request.Page, request.Size, request.EntryOrder,
            request.Direction);
        var data = await entryRepository.ListAsync(spec, cancellationToken);
        var itemCount = await entryRepository.CountAsync(spec, cancellationToken);
        var page = new Page<GetRatedEntryResponse>(data, request.Page, itemCount, request.Size);
        return new BaseResponse<Page<GetRatedEntryResponse>>(page);
    }
}