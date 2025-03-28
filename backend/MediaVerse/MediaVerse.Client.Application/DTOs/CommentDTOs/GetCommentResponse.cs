using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.DTOs.CommentDtos;

public class GetCommentResponse
{
    public Guid Id { get; set; }
    public Guid EntryId { get; set; }

    public string Username { get; set; } = null!;
    public string? UserProfile { get; set; }
    public Guid? UserId { get; set; }
    public string Content { get; set; } = null!;
    public int SubcommentCount { get; set; }
    public int Upvotes { get; set; }
    public int Downvotes { get; set; }
    public bool? UsersVote { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}