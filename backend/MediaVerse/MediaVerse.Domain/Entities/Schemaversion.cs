﻿namespace MediaVerse.Domain.Entities;

public partial class Schemaversion
{
    public int Schemaversionsid { get; set; }

    public string Scriptname { get; set; } = null!;

    public DateTime Applied { get; set; }
}
