namespace MediaVerse.Domain.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public Guid ProfilePictureId { get; set; }

    public virtual ICollection<AmaQuestion> AmaQuestions { get; set; } = new List<AmaQuestion>();

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Friendship> FriendshipUser2s { get; set; } = new List<Friendship>();

    public virtual ICollection<Friendship> FriendshipUsers { get; set; } = new List<Friendship>();

    public virtual ProfilePicture ProfilePicture { get; set; } = null!;

    public virtual ICollection<QuizTaking> QuizTakings { get; set; } = new List<QuizTaking>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
