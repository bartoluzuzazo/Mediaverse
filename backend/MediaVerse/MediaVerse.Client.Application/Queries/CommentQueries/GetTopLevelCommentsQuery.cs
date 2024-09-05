using MediatR;
using MediaVerse.Client.Application.DTOs.Comments;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.Specifications.CommentSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MediaVerse.Client.Application.Queries.CommentQueries;

public record GetTopLevelCommentsQuery(Guid EntryId, int Page, int Size, CommentOrder Order, OrderDirection Direction)
    : IRequest<BaseResponse<Page<GetCommentResponse>>>;

public class
    GetTopLevelCommentsQueryHandler : IRequestHandler<GetTopLevelCommentsQuery, BaseResponse<Page<GetCommentResponse>>>
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<Entry> _entryRepository;

    public GetTopLevelCommentsQueryHandler(IRepository<Entry> entryRepository, IRepository<Comment> commentRepository)
    {
        _entryRepository = entryRepository;
        _commentRepository = commentRepository;
    }

    public  async Task<BaseResponse<Page<GetCommentResponse>>> Handle(GetTopLevelCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var entry = await _entryRepository.GetByIdAsync(request.EntryId, cancellationToken);
        if (entry is null)
        {
            return new BaseResponse<Page<GetCommentResponse>>(new NotFoundException());
        }

        var spec = new GetCommentsSpecification(request.EntryId, null, null, request.Page, request.Size, request.Order,
            request.Direction);
        var data = await _commentRepository.ListAsync(spec, cancellationToken);
        var commentCount = await _commentRepository.CountAsync(spec, cancellationToken);
        var page = new Page<GetCommentResponse>()
        {
            Contents = data,
            PageCount = (commentCount + request.Size - 1) / request.Size,
            CurrentPage = request.Page
        };
        return new BaseResponse<Page<GetCommentResponse>>(page);
    }
}