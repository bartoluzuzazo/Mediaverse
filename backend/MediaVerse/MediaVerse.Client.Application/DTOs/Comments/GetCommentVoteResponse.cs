namespace MediaVerse.Client.Application.DTOs.Comments;

public class GetCommentVoteResponse
{
    public bool IsPositive { get; set; }
    public Guid CommentId { get; set; }
}