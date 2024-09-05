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

namespace MediaVerse.Client.Application.Commands.CommentCommands;

public record CreateVoteCommand : IRequest<BaseResponse<GetCommentVoteResponse>>
{
    public Guid CommentId { get; set; }
    public bool IsPositive { get; set; }
}

public class CreateVoteCommandHandler : UserAccessHandler,IRequestHandler<CreateVoteCommand, BaseResponse<GetCommentVoteResponse>>
{
    private readonly IUserAccessor _userAccessor;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<Vote> _voteRepository;

    public CreateVoteCommandHandler(IUserAccessor userAccessor, IRepository<User> userRepository,
        IRepository<Comment> commentRepository, IRepository<Vote> voteRepository) : base(userAccessor, userRepository)
    {
        _userAccessor = userAccessor;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
        _voteRepository = voteRepository;
    }

    public async Task<BaseResponse<GetCommentVoteResponse>> Handle(CreateVoteCommand request,
        CancellationToken cancellationToken)
    {
        
        var userResp = await GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetCommentVoteResponse>(userResp.Exception);
        }

        var user = userResp.Data!;

        var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment is null)
        {
            return new BaseResponse<GetCommentVoteResponse>(new NotFoundException());
        }

        var newVote = new Vote()
        {
            Comment = comment,
            User = user,
            IsPositive = request.IsPositive
        };
        var spec = new GetVoteByCommentAndAuthorIdsSpecification(comment.Id, user.Id);
        var conflictingVote = await _voteRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (conflictingVote is not null)
        {
            return new BaseResponse<GetCommentVoteResponse>(new ConflictException());
        }

        newVote = await _voteRepository.AddAsync(newVote, cancellationToken);
        var voteResponse = new GetCommentVoteResponse()
        {
            CommentId = newVote.CommentId,
            IsPositive = newVote.IsPositive,
        };

        return new BaseResponse<GetCommentVoteResponse>(voteResponse);
    }
}