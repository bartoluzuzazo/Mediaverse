using MediatR;
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

    
public record GetSubCommentsAuthorizedQuery(Guid CommentId, int Page, int Size, CommentOrder Order, OrderDirection Direction)
    : IRequest<BaseResponse<Page<GetCommentResponse>>>;

public class GetSubCommentsAuthorizedQueryHandler : IRequestHandler<GetSubCommentsAuthorizedQuery, BaseResponse<Page<GetCommentResponse>>>
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IUserAccessor _userAccessor;
    private readonly IRepository<User> _userRepository;

    public GetSubCommentsAuthorizedQueryHandler(IRepository<Comment> commentRepository, IUserAccessor userAccessor, IRepository<User> userRepository)
    {
        _commentRepository = commentRepository;
        _userAccessor = userAccessor;
        _userRepository = userRepository;
    }


    public async Task<BaseResponse<Page<GetCommentResponse>>> Handle(GetSubCommentsAuthorizedQuery request, CancellationToken cancellationToken)
    {
        var email = _userAccessor.Email;
        if (email is null)
        {
            return new BaseResponse<Page<GetCommentResponse>>(new ProblemException());
        }

        var user = await _userRepository.FirstOrDefaultAsync(new GetUserSpecification(email), cancellationToken);
        if (user is null)
        {
            return new BaseResponse<Page<GetCommentResponse>>(new NotFoundException());
        }
        
        var parent = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (parent is null)
        {
            return new BaseResponse<Page<GetCommentResponse>>(new NotFoundException());
        }

        var spec = new GetCommentsSpecification(null, request.CommentId, email, request.Page, request.Size,
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
