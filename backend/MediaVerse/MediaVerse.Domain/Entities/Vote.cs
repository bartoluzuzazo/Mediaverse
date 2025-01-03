namespace MediaVerse.Domain.Entities;

public partial class Vote
{
    public Guid UserId { get; set; }

    public Guid CommentId { get; set; }

    public bool IsPositive { get; set; }

    public virtual Comment Comment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
