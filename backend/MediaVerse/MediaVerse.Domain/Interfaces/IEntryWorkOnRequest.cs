namespace MediaVerse.Domain.Interfaces;

public interface IEntryWorkOnRequest
{
    public Guid Id { get; set; }
    public string Role { get; set; }
}