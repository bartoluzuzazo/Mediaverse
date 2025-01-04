namespace MediaVerse.Domain.Entities;

public partial class Episode
{
    public Guid Id { get; set; }

    public Guid SeriesId { get; set; }

    public string Synopsis { get; set; } = null!;

    public int SeasonNumber { get; set; }

    public int EpisodeNumber { get; set; }

    public virtual Entry IdNavigation { get; set; } = null!;

    public virtual Series Series { get; set; } = null!;
}
