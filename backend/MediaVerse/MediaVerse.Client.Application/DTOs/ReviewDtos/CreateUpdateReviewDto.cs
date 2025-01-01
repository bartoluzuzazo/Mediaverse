namespace MediaVerse.Client.Application.DTOs.ReviewDtos;

public class CreateUpdateReviewDto
{
    
    public string Content { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int Grade { get; set; }
}