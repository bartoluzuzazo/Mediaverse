namespace MediaVerse.Domain.Entities;

public partial class ProfilePicture
{
    public Guid Id { get; set; }

    public byte[] Picture { get; set; } = null!;

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
