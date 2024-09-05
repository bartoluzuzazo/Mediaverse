using System.ComponentModel.Design;
using MediatR;
using MediaVerse.Client.Application.Commands.Common;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.CommentSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.CommentCommands;

public record DeleteVoteCommand: IRequest<Exception?>
{
    public Guid CommentId { get; set; }
    
}
public class DeleteVoteCommandHandler : UserAccessHandler, IRequestHandler<DeleteVoteCommand, Exception?>
{
    private readonly IRepository<Vote> _voteRepository;
    public DeleteVoteCommandHandler(IUserAccessor userAccessor, IRepository<User> userRepository, IRepository<Vote> voteRepository) : base(userAccessor, userRepository)
    {
        _voteRepository = voteRepository;
    }

    public async  Task<Exception?> Handle(DeleteVoteCommand request, CancellationToken cancellationToken)
    {
        
        var userResp = await GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return userResp.Exception;
        }
        var user = userResp.Data!;
        var spec = new GetVoteByCommentAndAuthorIdsSpecification(request.CommentId, user.Id);
        var vote = await _voteRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (vote is not null)
        {
            await _voteRepository.DeleteAsync(vote, cancellationToken);
        }

        return null;
    }
}
