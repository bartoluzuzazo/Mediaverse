namespace MediaVerse.Client.Application.DTOs.WorkOnDTOs;

public class EntryWorkOnRequest
{
    public Guid Id { get; set; }
    public string Role { get; set; }
    public string? Details { get; set; }
}