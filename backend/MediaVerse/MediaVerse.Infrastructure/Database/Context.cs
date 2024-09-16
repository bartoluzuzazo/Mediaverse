using System;
using System.Collections.Generic;
using MediaVerse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MediaVerse.Infrastructure.Database;

public partial class Context : DbContext
{
    private readonly IConfiguration _configuration;

    public Context(DbContextOptions<Context> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<AmaQuestion> AmaQuestions { get; set; }

    public virtual DbSet<AmaSession> AmaSessions { get; set; }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorRole> AuthorRoles { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookGenre> BookGenres { get; set; }

    public virtual DbSet<CinematicGenre> CinematicGenres { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CoverPhoto> CoverPhotos { get; set; }

    public virtual DbSet<Developer> Developers { get; set; }

    public virtual DbSet<Entry> Entries { get; set; }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameGenre> GameGenres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MusicGenre> MusicGenres { get; set; }

    public virtual DbSet<ProfilePicture> ProfilePictures { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }

    public virtual DbSet<QuizTaking> QuizTakings { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vote> Votes { get; set; }

    public virtual DbSet<WorkOn> WorkOns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("album_pk");

            entity.ToTable("album");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Album)
                .HasForeignKey<Album>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("album_entry");

            entity.HasMany(d => d.MusicGenres).WithMany(p => p.Albums)
                .UsingEntity<Dictionary<string, object>>(
                    "AlbumMusicGenre",
                    r => r.HasOne<MusicGenre>().WithMany()
                        .HasForeignKey("MusicGenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("album_music_genre_music_genre"),
                    l => l.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("album_music_genre_album"),
                    j =>
                    {
                        j.HasKey("AlbumId", "MusicGenreId").HasName("album_music_genre_pk");
                        j.ToTable("album_music_genre");
                        j.IndexerProperty<Guid>("AlbumId").HasColumnName("album_id");
                        j.IndexerProperty<Guid>("MusicGenreId").HasColumnName("music_genre_id");
                    });
        });

        modelBuilder.Entity<AmaQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ama_question_pk");

            entity.ToTable("ama_question");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AmaSessionId).HasColumnName("ama_session_id");
            entity.Property(e => e.Answer)
                .HasMaxLength(3000)
                .HasColumnName("answer");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.AmaSession).WithMany(p => p.AmaQuestions)
                .HasForeignKey(d => d.AmaSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("question_ama_session");

            entity.HasOne(d => d.User).WithMany(p => p.AmaQuestions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("question_user");
        });

        modelBuilder.Entity<AmaSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ama_session_pk");

            entity.ToTable("ama_session");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.End)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end");
            entity.Property(e => e.PlannedEnd)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("planned_end");
            entity.Property(e => e.Start)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start");

            entity.HasOne(d => d.Author).WithMany(p => p.AmaSessions)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ama_session_author");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("answer_pk");

            entity.ToTable("answer");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IsCorrect).HasColumnName("is_correct");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.Text)
                .HasMaxLength(20)
                .HasColumnName("text");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("answer_question");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("article_pk");

            entity.ToTable("article");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Timestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Articles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("article_user");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("author_pk");

            entity.ToTable("author");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.ProfilePictureId).HasColumnName("profile_picture_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(150)
                .HasColumnName("surname");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ProfilePicture).WithMany(p => p.Authors)
                .HasForeignKey(d => d.ProfilePictureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("author_profile_picture");

            entity.HasOne(d => d.User).WithMany(p => p.Authors)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("author_user");
        });

        modelBuilder.Entity<AuthorRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("author_role_pk");

            entity.ToTable("author_role");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_pk");

            entity.ToTable("book");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("isbn");
            entity.Property(e => e.Synopsis).HasColumnName("synopsis");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Book)
                .HasForeignKey<Book>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("book_entry");

            entity.HasMany(d => d.BookGenres).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookBookGenre",
                    r => r.HasOne<BookGenre>().WithMany()
                        .HasForeignKey("BookGenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("book_book_genre_book_genre"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("book_book_genre_book"),
                    j =>
                    {
                        j.HasKey("BookId", "BookGenreId").HasName("book_book_genre_pk");
                        j.ToTable("book_book_genre");
                        j.IndexerProperty<Guid>("BookId").HasColumnName("book_id");
                        j.IndexerProperty<Guid>("BookGenreId").HasColumnName("book_genre_id");
                    });
        });

        modelBuilder.Entity<BookGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_genre_pk");

            entity.ToTable("book_genre");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CinematicGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cinematic_genre_pk");

            entity.ToTable("cinematic_genre");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("id");

            entity.ToTable("comment");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");
            entity.Property(e => e.EntryId).HasColumnName("entry_id");
            entity.Property(e => e.ParentCommentId).HasColumnName("parent_comment_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Entry).WithMany(p => p.Comments)
                .HasForeignKey(d => d.EntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comment_entry");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("comment_comment");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comment_user");
        });

        modelBuilder.Entity<CoverPhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cover_photo_pk");

            entity.ToTable("cover_photo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Photo).HasColumnName("photo");
        });

        modelBuilder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("developer_pk");

            entity.ToTable("developer");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Entry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("entry_pk");

            entity.ToTable("entry");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CoverPhotoId).HasColumnName("cover_photo_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Release).HasColumnName("release");

            entity.HasOne(d => d.CoverPhoto).WithMany(p => p.Entries)
                .HasForeignKey(d => d.CoverPhotoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entry_cover_photo");

            entity.Property(e => e.SearchVector).HasColumnName("search_vector");
            
            entity.HasGeneratedTsVectorColumn(
                    p => p.SearchVector, "english", p => new { p.Name, p.Description }).HasIndex(p => p.SearchVector)
                .HasMethod("GIN");
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("episode_pk");

            entity.ToTable("episode");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.SeriesId).HasColumnName("series_id");
            entity.Property(e => e.Synopsis).HasColumnName("synopsis");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Episode)
                .HasForeignKey<Episode>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("episode_entry");

            entity.HasOne(d => d.Series).WithMany(p => p.Episodes)
                .HasForeignKey(d => d.SeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("episode_series");
        });

        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.User2Id }).HasName("friendship_pk");

            entity.ToTable("friendship");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.User2Id).HasColumnName("user_2_id");
            entity.Property(e => e.Approved).HasColumnName("approved");

            entity.HasOne(d => d.User2).WithMany(p => p.FriendshipUser2s)
                .HasForeignKey(d => d.User2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friendship_user2");

            entity.HasOne(d => d.User).WithMany(p => p.FriendshipUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friendship_user");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_pk");

            entity.ToTable("game");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Synopsis).HasColumnName("synopsis");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Game)
                .HasForeignKey<Game>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_entry");

            entity.HasMany(d => d.Developers).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameDeveloper",
                    r => r.HasOne<Developer>().WithMany()
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_developer_developer"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_developer_game"),
                    j =>
                    {
                        j.HasKey("GameId", "DeveloperId").HasName("game_developer_pk");
                        j.ToTable("game_developer");
                        j.IndexerProperty<Guid>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<Guid>("DeveloperId").HasColumnName("developer_id");
                    });

            entity.HasMany(d => d.GameGenres).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameGameGenre",
                    r => r.HasOne<GameGenre>().WithMany()
                        .HasForeignKey("GameGenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_game_genre_game_genre"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_game_genre_game"),
                    j =>
                    {
                        j.HasKey("GameId", "GameGenreId").HasName("game_game_genre_pk");
                        j.ToTable("game_game_genre");
                        j.IndexerProperty<Guid>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<Guid>("GameGenreId").HasColumnName("game_genre_id");
                    });
        });

        modelBuilder.Entity<GameGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_genre_pk");

            entity.ToTable("game_genre");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movie_pk");

            entity.ToTable("movie");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Synopsis).HasColumnName("synopsis");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Movie)
                .HasForeignKey<Movie>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movie_entry");

            entity.HasMany(d => d.CinematicGenres).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieCinematicGenre",
                    r => r.HasOne<CinematicGenre>().WithMany()
                        .HasForeignKey("CinematicGenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("movie_cinematic_genre_cinematic_genre"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("movie_cinematic_genre_movie"),
                    j =>
                    {
                        j.HasKey("MovieId", "CinematicGenreId").HasName("movie_cinematic_genre_pk");
                        j.ToTable("movie_cinematic_genre");
                        j.IndexerProperty<Guid>("MovieId").HasColumnName("movie_id");
                        j.IndexerProperty<Guid>("CinematicGenreId").HasColumnName("cinematic_genre_id");
                    });
        });

        modelBuilder.Entity<MusicGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("music_genre_pk");

            entity.ToTable("music_genre");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ProfilePicture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("profile_picture_pk");

            entity.ToTable("profile_picture");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Picture).HasColumnName("picture");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quiz_pk");

            entity.ToTable("quiz");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EntryId).HasColumnName("entry_id");

            entity.HasOne(d => d.Entry).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.EntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("quiz_entry");
        });

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quiz_question_pk");

            entity.ToTable("quiz_question");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AddedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("added_at");
            entity.Property(e => e.QuizId).HasColumnName("quiz_id");
            entity.Property(e => e.Text)
                .HasMaxLength(200)
                .HasColumnName("text");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizQuestions)
                .HasForeignKey(d => d.QuizId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("question_quiz");
        });

        modelBuilder.Entity<QuizTaking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quiz_taking_pk");

            entity.ToTable("quiz_taking");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.QuizId).HasColumnName("quiz_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Takenat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("takenat");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizTakings)
                .HasForeignKey(d => d.QuizId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("quiz_taking_quiz");

            entity.HasOne(d => d.User).WithMany(p => p.QuizTakings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("quiz_taking_user");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rating_pk");

            entity.ToTable("rating");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.EntryId).HasColumnName("entry_id");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Entry).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.EntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rating_entry");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rating_user");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.EntryId }).HasName("review_pk");

            entity.ToTable("review");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.EntryId).HasColumnName("entry_id");
            entity.Property(e => e.Content)
                .HasMaxLength(2000)
                .HasColumnName("content");

            entity.HasOne(d => d.Entry).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.EntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("review_entry");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("review_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pk");

            entity.ToTable("role");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");

            entity.HasMany(d => d.Users).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RoleUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("role_user_user"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("role_user_role"),
                    j =>
                    {
                        j.HasKey("RoleId", "UserId").HasName("role_user_pk");
                        j.ToTable("role_user");
                        j.IndexerProperty<Guid>("RoleId").HasColumnName("role_id");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                    });
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("series_pk");

            entity.ToTable("series");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Series)
                .HasForeignKey<Series>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("series_entry");

            entity.HasMany(d => d.CinematicGenres).WithMany(p => p.Series)
                .UsingEntity<Dictionary<string, object>>(
                    "SeriesCinematicGenre",
                    r => r.HasOne<CinematicGenre>().WithMany()
                        .HasForeignKey("CinematicGenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("series_cinematic_genre_cinematic_genre"),
                    l => l.HasOne<Series>().WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("series_cinematic_genre_series"),
                    j =>
                    {
                        j.HasKey("SeriesId", "CinematicGenreId").HasName("series_cinematic_genre_pk");
                        j.ToTable("series_cinematic_genre");
                        j.IndexerProperty<Guid>("SeriesId").HasColumnName("series_id");
                        j.IndexerProperty<Guid>("CinematicGenreId").HasColumnName("cinematic_genre_id");
                    });
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("song_pk");

            entity.ToTable("song");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Lyrics).HasColumnName("lyrics");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Song)
                .HasForeignKey<Song>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("song_entry");

            entity.HasMany(d => d.Albums).WithMany(p => p.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "SongAlbum",
                    r => r.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("song_album_album"),
                    l => l.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("song_album_song"),
                    j =>
                    {
                        j.HasKey("SongId", "AlbumId").HasName("song_album_pk");
                        j.ToTable("song_album");
                        j.IndexerProperty<Guid>("SongId").HasColumnName("song_id");
                        j.IndexerProperty<Guid>("AlbumId").HasColumnName("album_id");
                    });

            entity.HasMany(d => d.MusicGenres).WithMany(p => p.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "SongMusicGenre",
                    r => r.HasOne<MusicGenre>().WithMany()
                        .HasForeignKey("MusicGenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("song_music_genre_music_genre"),
                    l => l.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("song_music_genre_song"),
                    j =>
                    {
                        j.HasKey("SongId", "MusicGenreId").HasName("song_music_genre_pk");
                        j.ToTable("song_music_genre");
                        j.IndexerProperty<Guid>("SongId").HasColumnName("song_id");
                        j.IndexerProperty<Guid>("MusicGenreId").HasColumnName("music_genre_id");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pk");

            entity.ToTable("user");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .HasColumnName("password_hash");
            entity.Property(e => e.ProfilePictureId).HasColumnName("profile_picture_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.ProfilePicture).WithMany(p => p.Users)
                .HasForeignKey(d => d.ProfilePictureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_profile_picture");

            entity.HasMany(d => d.Quizzes).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "QuizWriting",
                    r => r.HasOne<Quiz>().WithMany()
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("quiz_writing_quiz"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("quiz_writing_user"),
                    j =>
                    {
                        j.HasKey("UserId", "QuizId").HasName("quiz_writing_pk");
                        j.ToTable("quiz_writing");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<Guid>("QuizId").HasColumnName("quiz_id");
                    });
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.CommentId }).HasName("vote_pk");

            entity.ToTable("vote");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.IsPositive).HasColumnName("is_positive");

            entity.HasOne(d => d.Comment).WithMany(p => p.Votes)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vote_comment");

            entity.HasOne(d => d.User).WithMany(p => p.Votes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vote_user");
        });

        modelBuilder.Entity<WorkOn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("work_on_pk");

            entity.ToTable("work_on");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.AuthorRoleId).HasColumnName("author_role_id");
            entity.Property(e => e.EntryId).HasColumnName("entry_id");

            entity.HasOne(d => d.Author).WithMany(p => p.WorkOns)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("work_on_author");

            entity.HasOne(d => d.AuthorRole).WithMany(p => p.WorkOns)
                .HasForeignKey(d => d.AuthorRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("work_on_author_role");

            entity.HasOne(d => d.Entry).WithMany(p => p.WorkOns)
                .HasForeignKey(d => d.EntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("work_on_entry");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}