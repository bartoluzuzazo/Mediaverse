﻿using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class Article
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual User User { get; set; } = null!;
}
