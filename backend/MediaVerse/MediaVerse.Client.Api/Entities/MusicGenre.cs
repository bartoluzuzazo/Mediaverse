using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class MusicGenre
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
