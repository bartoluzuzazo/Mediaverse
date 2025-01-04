using NpgsqlTypes;

namespace MediaVerse.Domain.Entities;

public partial class Article
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTime Timestamp { get; set; }

    public string Lede { get; set; } = null!;

    public NpgsqlTsVector? SearchVector { get; set; }

    public virtual User User { get; set; } = null!;
}
