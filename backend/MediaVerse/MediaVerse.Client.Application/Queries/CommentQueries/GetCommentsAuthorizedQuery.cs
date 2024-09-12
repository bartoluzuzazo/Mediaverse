using MediatR;
using MediaVerse.Client.Application.DTOs.CommentDtos;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.CommentSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Queries.CommentQueries;

public record GetCommentsAuthorizedQuery : IRequest<BaseResponse<Page<GetCommentResponse>>>
{
    public Guid? EntryId { get; set; }
    public Guid? ParentId { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public CommentOrder Order { get; set; }
    public OrderDirection Direction { get; set; }
}

public class GetCommentsAuthorizedQueryHandler(
    IRepository<Comment> commentRepository,
    IRepository<Entry> entryRepository,
    IUserService userService)
    : IRequestHandler<GetCommentsAuthorizedQuery,
        BaseResponse<Page<GetCommentResponse>>>
{
    public async Task<BaseResponse<Page<GetCommentResponse>>> Handle(GetCommentsAuthorizedQuery request,
        CancellationToken cancellationToken)
    {

        
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<Page<GetCommentResponse>>(userResp.Exception);
        }

        var email = userResp.Data!.Email;

        if (request.EntryId is not null)
        {
            var entry = await entryRepository.GetByIdAsync(request.EntryId, cancellationToken);
            if (entry is null)
            {
                return new BaseResponse<Page<GetCommentResponse>>(new NotFoundException());
            }
        }

        if (request.ParentId is not null)
        {
            var parent = await commentRepository.GetByIdAsync(request.ParentId, cancellationToken);
            if (parent is null)
            {
                return new BaseResponse<Page<GetCommentResponse>>(new NotFoundException());
            }
        }

        var spec = new GetCommentsSpecification(request.EntryId, request.ParentId, email, request.Page, request.Size, request.Order,
            request.Direction);
        var data = await commentRepository.ListAsync(spec, cancellationToken);
        var commentCount = await commentRepository.CountAsync(spec, cancellationToken);
        var page = new Page<GetCommentResponse>()
        {
            Contents = data,
            PageCount = (commentCount + request.Size - 1) / request.Size,
            CurrentPage = request.Page
        };
        return new BaseResponse<Page<GetCommentResponse>>(page);
    }
}