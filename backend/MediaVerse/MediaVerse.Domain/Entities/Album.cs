namespace MediaVerse.Domain.Entities;

public partial class Album
{
    public Guid Id { get; set; }

    public virtual Entry IdNavigation { get; set; } = null!;

    public virtual ICollection<MusicGenre> MusicGenres { get; set; } = new List<MusicGenre>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
