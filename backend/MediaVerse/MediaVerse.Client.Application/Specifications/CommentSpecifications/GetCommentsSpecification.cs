using Ardalis.Specification;
using MediaVerse.Client.Application.DTOs.CommentDtos;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.CommentSpecifications;

public class GetCommentsSpecification : Specification<Comment, GetCommentResponse>
{
    public GetCommentsSpecification(Guid? entryId, Guid? parentCommentId, string? userEmail, int page, int size,
        CommentOrder order, OrderDirection direction)
    {
        if (entryId is not null)
        {
            Query.Where(c => c.EntryId == entryId && c.ParentComment == null);
        }

        if (parentCommentId is not null)
        {
            Query.Where(c => c.ParentCommentId == parentCommentId);
        }

        Query.Select(comment => new GetCommentResponse()
            {
                Id = comment.Id,
                EntryId = comment.EntryId,
                Username = comment.DeletedAt == null ? comment.User.Username : "[deleted]",
                UserId = comment.DeletedAt == null ? comment.UserId : null,
                UserProfile = comment.User.ProfilePicture == null || comment.DeletedAt != null
                    ? null
                    : Convert.ToBase64String(comment.User.ProfilePicture.Picture),
                Content = comment.DeletedAt == null ? comment.Content : "[deleted]",
                SubcommentCount = comment.InverseParentComment.Count(),
                Upvotes = comment.Votes.Count(c => c.IsPositive),
                Downvotes = comment.Votes.Count(c => !c.IsPositive),
                CreatedAt = comment.CreatedAt,
                IsDeleted = comment.DeletedAt != null,

                UsersVote = userEmail == null
                    ? null
                    : comment.Votes.Any(v => v.User.Email == userEmail)
                        ? comment.Votes
                            .Where(v => v.User.Email == userEmail)
                            .Select(v => v.IsPositive)
                            .FirstOrDefault()
                        : null
            })
            .OrderComments(order, direction)
            .Paginate(page, size);
    }
}