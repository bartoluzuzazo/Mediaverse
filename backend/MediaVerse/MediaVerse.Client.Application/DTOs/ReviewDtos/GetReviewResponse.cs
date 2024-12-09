namespace MediaVerse.Client.Application.DTOs.ReviewDtos;

public class GetReviewResponse
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;
    
    public string ProfilePicture { get; set; } = null!;

    public Guid EntryId { get; set; }

    public string Content { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int Grade { get; set; }
    
}