namespace MediaVerse.Domain.Entities;

public partial class Comment
{
    public Guid Id { get; set; }

    public Guid EntryId { get; set; }

    public Guid? ParentCommentId { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateOnly? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Entry Entry { get; set; } = null!;

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Comment? ParentComment { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
