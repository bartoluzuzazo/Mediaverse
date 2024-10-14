namespace MediaVerse.Domain.Entities;

public partial class AmaSession
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public virtual ICollection<AmaQuestion> AmaQuestions { get; set; } = new List<AmaQuestion>();

    public virtual Author Author { get; set; } = null!;
}
