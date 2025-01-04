namespace MediaVerse.Client.Application.DTOs.ArticleDtos;

public class GetArticlePreviewResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public Guid UserId { get; set; }
    
    public string AuthorUsername { get; set; } = null!;
    
    public string AuthorPicture { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public string Lede { get; set; } = null!;
}