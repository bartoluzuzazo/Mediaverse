namespace MediaVerse.Domain.Entities;

public partial class CoverPhoto
{
    public Guid Id { get; set; }

    public byte[] Photo { get; set; } = null!;

    public virtual ICollection<Entry> Entries { get; set; } = new List<Entry>();
}
