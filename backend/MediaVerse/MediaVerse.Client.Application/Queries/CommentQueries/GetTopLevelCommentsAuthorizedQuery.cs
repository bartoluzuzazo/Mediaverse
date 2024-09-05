using MediatR;
using MediaVerse.Client.Application.Commands.Common;
using MediaVerse.Client.Application.DTOs.Comments;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.CommentSpecifications;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Queries.CommentQueries;

public record GetTopLevelCommentsAuthorizedQuery : IRequest<BaseResponse<Page<GetCommentResponse>>>
{
    public Guid? EntryId { get; set; }
    public Guid? ParentId { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public CommentOrder Order { get; set; }
    public OrderDirection Direction { get; set; }
}

public class GetTopLevelCommentsAuthorizedQueryHandler : UserAccessHandler,IRequestHandler<GetTopLevelCommentsAuthorizedQuery,
    BaseResponse<Page<GetCommentResponse>>>
{
    private readonly IUserAccessor _userAccessor;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<Entry> _entryRepository;


    public GetTopLevelCommentsAuthorizedQueryHandler(IUserAccessor userAccessor, IRepository<User> userRepository, IRepository<Comment> commentRepository, IRepository<Entry> entryRepository) : base(userAccessor, userRepository)
    {
        _userAccessor = userAccessor;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
        _entryRepository = entryRepository;
    }

    public async Task<BaseResponse<Page<GetCommentResponse>>> Handle(GetTopLevelCommentsAuthorizedQuery request,
        CancellationToken cancellationToken)
    {

        
        var userResp = await GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<Page<GetCommentResponse>>(userResp.Exception);
        }

        var email = userResp.Data!.Email;

        if (request.EntryId is not null)
        {
            var entry = await _entryRepository.GetByIdAsync(request.EntryId, cancellationToken);
            if (entry is null)
            {
                return new BaseResponse<Page<GetCommentResponse>>(new NotFoundException());
            }
        }

        if (request.ParentId is not null)
        {
            var parent = await _commentRepository.GetByIdAsync(request.ParentId, cancellationToken);
            if (parent is null)
            {
                return new BaseResponse<Page<GetCommentResponse>>(new NotFoundException());
            }
        }

        var spec = new GetCommentsSpecification(request.EntryId, null, email, request.Page, request.Size, request.Order,
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