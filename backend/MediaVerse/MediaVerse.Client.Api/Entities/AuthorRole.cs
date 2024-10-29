using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class AuthorRole
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<WorkOn> WorkOns { get; set; } = new List<WorkOn>();
}
