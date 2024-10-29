using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class WorkOn
{
    public Guid AuthorId { get; set; }

    public Guid EntryId { get; set; }

    public Guid AuthorRoleId { get; set; }

    public Guid Id { get; set; }

    public string? Details { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual AuthorRole AuthorRole { get; set; } = null!;

    public virtual Entry Entry { get; set; } = null!;
}
