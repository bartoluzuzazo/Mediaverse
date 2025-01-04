namespace MediaVerse.Domain.Entities;

public partial class Book
{
    public Guid Id { get; set; }

    public string Isbn { get; set; } = null!;

    public string Synopsis { get; set; } = null!;

    public virtual Entry IdNavigation { get; set; } = null!;

    public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}
