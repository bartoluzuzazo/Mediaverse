namespace MediaVerse.Client.Application.DTOs.ArticleDtos;

public class GetArticleResponse
{
    
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public Guid UserId { get; set; }
    
    public string authorUsername { get; set; } = null!;
    
    public string authorPicture { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public string Lede { get; set; } = null!;
}