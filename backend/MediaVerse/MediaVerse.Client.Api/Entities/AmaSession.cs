using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class AmaSession
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<AmaQuestion> AmaQuestions { get; set; } = new List<AmaQuestion>();

    public virtual Author Author { get; set; } = null!;
}
