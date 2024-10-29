using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class CinematicGenre
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();

    public virtual ICollection<Series> Series { get; set; } = new List<Series>();
}
