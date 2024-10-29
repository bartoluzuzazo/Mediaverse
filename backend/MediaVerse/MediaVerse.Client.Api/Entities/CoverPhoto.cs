using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class CoverPhoto
{
    public Guid Id { get; set; }

    public byte[] Photo { get; set; } = null!;

    public virtual ICollection<Entry> Entries { get; set; } = new List<Entry>();
}
