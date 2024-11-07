using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.DTOs.WorkOnDTOs;

public class EntryWorkOnRequest : IEntryWorkOnRequest
{
    public Guid Id { get; set; }
    public string Role { get; set; }
}