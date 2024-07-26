namespace MediaVerse.Domain.Entities;

public partial class Author
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Bio { get; set; } = null!;

    public Guid? UserId { get; set; }

    public Guid ProfilePictureId { get; set; }

    public virtual ICollection<AmaSession> AmaSessions { get; set; } = new List<AmaSession>();

    public virtual ProfilePicture ProfilePicture { get; set; } = null!;

    public virtual User? User { get; set; }

    public virtual ICollection<WorkOn> WorkOns { get; set; } = new List<WorkOn>();
}
