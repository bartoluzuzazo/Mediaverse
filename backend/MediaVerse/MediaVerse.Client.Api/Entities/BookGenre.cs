using System;
using System.Collections.Generic;

namespace MediaVerse.Client.Api.Entities;

public partial class BookGenre
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
