namespace MediaVerse.Domain.Entities;

public partial class AmaQuestion
{
    public Guid Id { get; set; }

    public Guid AmaSessionId { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; set; } = null!;

    public string? Answer { get; set; }

    public virtual AmaSession AmaSession { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
