namespace MediaVerse.Domain.Entities;

public partial class Answer
{
    public Guid Id { get; set; }

    public bool IsCorrect { get; set; }

    public string Text { get; set; } = null!;

    public Guid QuestionId { get; set; }

    public virtual QuizQuestion Question { get; set; } = null!;
}
