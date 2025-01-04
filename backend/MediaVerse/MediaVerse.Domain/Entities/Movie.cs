namespace MediaVerse.Domain.Entities;

public partial class Movie
{
    public Guid Id { get; set; }

    public string Synopsis { get; set; } = null!;

    public virtual Entry IdNavigation { get; set; } = null!;

    public virtual ICollection<CinematicGenre> CinematicGenres { get; set; } = new List<CinematicGenre>();
}
