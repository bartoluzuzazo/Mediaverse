using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class QuizTaking
{
    public Guid UserId { get; set; }

    public Guid QuizId { get; set; }

    public int Score { get; set; }

    public Guid Id { get; set; }

    public DateTime Takenat { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
