using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class Series
{
    public Guid Id { get; set; }

    public virtual ICollection<Episode> Episodes { get; set; } = new List<Episode>();

    public virtual Entry IdNavigation { get; set; } = null!;

    public virtual ICollection<CinematicGenre> CinematicGenres { get; set; } = new List<CinematicGenre>();
}
