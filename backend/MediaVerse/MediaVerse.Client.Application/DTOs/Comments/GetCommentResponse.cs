namespace MediaVerse.Client.Application.DTOs.Comments;

public class GetCommentResponse
{
    public Guid Id { get; set; }
    public Guid EntryId { get; set; }
    public string Username = null!;
    public string? UserProfile { get; set; }
    public string Content { get; set; } = null!;
    public int SubcommentCount { get; set; }
    public int VoteSum { get; set; }
    public bool? UsersVote { get; set; }
}