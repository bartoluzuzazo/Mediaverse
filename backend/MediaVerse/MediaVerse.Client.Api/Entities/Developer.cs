using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class Developer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
