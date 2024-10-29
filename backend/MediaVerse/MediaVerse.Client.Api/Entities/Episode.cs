using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class Episode
{
    public Guid Id { get; set; }

    public Guid SeriesId { get; set; }

    public string Synopsis { get; set; } = null!;

    public virtual Entry IdNavigation { get; set; } = null!;

    public virtual Series Series { get; set; } = null!;
}
