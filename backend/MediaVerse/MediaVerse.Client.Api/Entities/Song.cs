using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class Song
{
    public Guid Id { get; set; }

    public string? Lyrics { get; set; }

    public virtual Entry IdNavigation { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<MusicGenre> MusicGenres { get; set; } = new List<MusicGenre>();
}
