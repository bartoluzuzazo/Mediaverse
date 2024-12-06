namespace MediaVerse.Domain.Entities;

public partial class Quiz
{
    public Guid EntryId { get; set; }

    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Entry Entry { get; set; } = null!;

    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();

    public virtual ICollection<QuizTaking> QuizTakings { get; set; } = new List<QuizTaking>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
