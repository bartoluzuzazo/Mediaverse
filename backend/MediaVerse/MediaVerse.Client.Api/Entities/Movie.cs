using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class Movie
{
    public Guid Id { get; set; }

    public string Synopsis { get; set; } = null!;

    public virtual Entry IdNavigation { get; set; } = null!;

    public virtual ICollection<CinematicGenre> CinematicGenres { get; set; } = new List<CinematicGenre>();
}
