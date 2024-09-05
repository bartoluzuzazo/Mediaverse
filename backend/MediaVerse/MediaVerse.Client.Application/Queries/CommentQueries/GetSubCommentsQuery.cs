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

public record GetSubCommentsQuery(Guid CommentId, int Page, int Size, CommentOrder Order, OrderDirection Direction)
    : IRequest<BaseResponse<Page<GetCommentResponse>>>;

public class GetSubCommentsQueryHandler : IRequestHandler<GetSubCommentsQuery, BaseResponse<Page<GetCommentResponse>>>
{
    private readonly IRepository<Comment> _commentRepository;

    public GetSubCommentsQueryHandler(IRepository<Comment> commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<BaseResponse<Page<GetCommentResponse>>> Handle(GetSubCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var parent = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (parent is null)
        {
            return new BaseResponse<Page<GetCommentResponse>>(new NotFoundException());
        }

        var spec = new GetCommentsSpecification(null, request.CommentId, null, request.Page, request.Size,
            request.Order, request.Direction);
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