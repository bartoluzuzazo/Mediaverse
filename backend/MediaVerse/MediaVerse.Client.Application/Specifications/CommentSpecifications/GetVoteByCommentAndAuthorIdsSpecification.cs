using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.CommentSpecifications;

public class GetVoteByCommentAndAuthorIdsSpecification : Specification<Vote>
{
    public GetVoteByCommentAndAuthorIdsSpecification(Guid commentId, Guid userId)
    {
        Query.Where(vote => vote.UserId == userId && vote.CommentId == commentId)
            .Include(vote => vote.Comment);
    }
}