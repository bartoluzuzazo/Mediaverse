using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class Album
{
    public Guid Id { get; set; }

    public virtual Entry IdNavigation { get; set; } = null!;

    public virtual ICollection<MusicGenre> MusicGenres { get; set; } = new List<MusicGenre>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
