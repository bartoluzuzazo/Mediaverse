using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class Rating
{
    public Guid Id { get; set; }

    public int Grade { get; set; }

    public Guid UserId { get; set; }

    public Guid EntryId { get; set; }

    public DateTime Modifiedat { get; set; }

    public virtual Entry Entry { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
