using MediaVerse.Domain.Entities;

namespace MediaVerse.Domain.Interfaces;

public interface IEntryCommand
{
    public Entry Entry { get; set; }
    public CoverPhoto Photo { get; set; }
}