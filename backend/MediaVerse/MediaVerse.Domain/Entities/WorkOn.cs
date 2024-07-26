namespace MediaVerse.Domain.Entities;

public partial class WorkOn
{
    public Guid AuthorId { get; set; }

    public Guid EntryId { get; set; }

    public Guid AuthorRoleId { get; set; }

    public Guid Id { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual AuthorRole AuthorRole { get; set; } = null!;

    public virtual Entry Entry { get; set; } = null!;
}
