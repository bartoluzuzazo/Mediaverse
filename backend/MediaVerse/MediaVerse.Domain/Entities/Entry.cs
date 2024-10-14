using NpgsqlTypes;

namespace MediaVerse.Domain.Entities;

public partial class Entry
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly Release { get; set; }

    public Guid CoverPhotoId { get; set; }

    public NpgsqlTsVector? SearchVector { get; set; }

    public string? Type { get; set; }

    public virtual Album? Album { get; set; }

    public virtual Book? Book { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual CoverPhoto CoverPhoto { get; set; } = null!;

    public virtual Episode? Episode { get; set; }

    public virtual Game? Game { get; set; }

    public virtual Movie? Movie { get; set; }

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Series? Series { get; set; }

    public virtual Song? Song { get; set; }

    public virtual ICollection<WorkOn> WorkOns { get; set; } = new List<WorkOn>();
}
