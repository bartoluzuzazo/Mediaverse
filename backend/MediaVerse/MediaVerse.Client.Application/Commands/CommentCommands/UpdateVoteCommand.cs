using MediatR;
using MediaVerse.Client.Application.DTOs.CommentDtos;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.CommentSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.CommentCommands;

public record UpdateVoteCommand : IRequest<BaseResponse<GetCommentVoteResponse>>
{
    public PutVoteDto VoteDto { get; set; } = null!;
    public Guid CommentId { get; set; }
}

public class UpdateVoteCommandHandler : IRequestHandler<UpdateVoteCommand, BaseResponse<GetCommentVoteResponse>>
{
    private readonly IRepository<Vote> _voteRepository;
    private readonly IUserService _userService;

    public UpdateVoteCommandHandler(IRepository<Vote> voteRepository, IUserService userService)
    {
        _voteRepository = voteRepository;
        _userService = userService;
    }

    public async Task<BaseResponse<GetCommentVoteResponse>> Handle(UpdateVoteCommand request,
        CancellationToken cancellationToken)
    {
        var userResp = await _userService.GetCurrentUserAsync(cancellationToken);
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

        vote.IsPositive = request.VoteDto.IsPositive;
        await _voteRepository.SaveChangesAsync(cancellationToken);
        var responseVote = new GetCommentVoteResponse()
        {
            CommentId = request.CommentId,
            IsPositive = request.VoteDto.IsPositive
        };
        return new BaseResponse<GetCommentVoteResponse>(responseVote);
    }
}