namespace MediaVerse.Client.Application.DTOs.CommentDtos;

public class GetCommentVoteResponse
{
    public bool IsPositive { get; set; }
    public Guid CommentId { get; set; }
}