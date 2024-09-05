using MediatR;
using MediaVerse.Client.Application.Commands.Common;
using MediaVerse.Client.Application.DTOs.Comments;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.CommentSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.CommentCommands;

public record UpdateVoteCommand: IRequest<BaseResponse<GetCommentVoteResponse>>
{
    public Guid CommentId { get; set; }
    public bool IsPositive { get; set; }
    
}

public class UpdateVoteCommandHandler : UserAccessHandler,IRequestHandler<UpdateVoteCommand,BaseResponse<GetCommentVoteResponse>>
{
    private readonly IRepository<Vote> _voteRepository;
    public UpdateVoteCommandHandler(IUserAccessor userAccessor, IRepository<User> userRepository, IRepository<Vote> voteRepository) : base(userAccessor, userRepository)
    {
        _voteRepository = voteRepository;
    }

    public async Task<BaseResponse<GetCommentVoteResponse>> Handle(UpdateVoteCommand request, CancellationToken cancellationToken)
    {
        var userResp = await GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetCommentVoteResponse>(userResp.Exception);
        }

        var spec = new GetVoteByCommentAndAuthorIdsSpecification(request.CommentId, userResp.Data!.Id);
        var vote = await _voteRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (vote is null)
        {
            return new BaseResponse<GetCommentVoteResponse>(new NotFoundException());
        }

        vote.IsPositive = request.IsPositive;
        await _voteRepository.SaveChangesAsync(cancellationToken);
        var responseVote = new GetCommentVoteResponse()
        {
            CommentId = request.CommentId,
            IsPositive = request.IsPositive
        };
        return new BaseResponse<GetCommentVoteResponse>(responseVote);

    }
}