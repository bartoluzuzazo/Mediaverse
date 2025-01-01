namespace MediaVerse.Client.Application.DTOs.ReviewDtos;

public class GetReviewPreviewResponse
{
    
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string ProfilePicture { get; set; } = null!;

    public Guid EntryId { get; set; }

    public string Title { get; set; } = null!;

    public int Grade { get; set; }
}