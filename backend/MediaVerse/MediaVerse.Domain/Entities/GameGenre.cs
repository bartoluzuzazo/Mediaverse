namespace MediaVerse.Domain.Entities;

public partial class GameGenre
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
