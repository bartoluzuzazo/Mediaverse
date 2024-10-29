using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class Friendship
{
    public Guid UserId { get; set; }

    public Guid User2Id { get; set; }

    public bool Approved { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual User User2 { get; set; } = null!;
}
