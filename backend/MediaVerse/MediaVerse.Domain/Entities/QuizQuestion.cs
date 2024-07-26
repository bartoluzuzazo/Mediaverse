namespace MediaVerse.Domain.Entities;

public partial class QuizQuestion
{
    public Guid Id { get; set; }

    public Guid QuizId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime AddedAt { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Quiz Quiz { get; set; } = null!;
}
