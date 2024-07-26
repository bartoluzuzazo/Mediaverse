namespace MediaVerse.Domain.Entities;

public partial class Review
{
    public Guid UserId { get; set; }

    public Guid EntryId { get; set; }

    public string Content { get; set; } = null!;

    public virtual Entry Entry { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
