namespace MediaVerse.Domain.Entities;

public partial class Game
{
    public Guid Id { get; set; }

    public string Synopsis { get; set; } = null!;

    public virtual Entry IdNavigation { get; set; } = null!;

    public virtual ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
}
