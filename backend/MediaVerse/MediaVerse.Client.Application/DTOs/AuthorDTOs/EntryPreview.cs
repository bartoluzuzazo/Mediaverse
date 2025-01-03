namespace MediaVerse.Client.Application.DTOs.AuthorDTOs;

public class EntryPreview
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal AvgRating { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public byte[] CoverPhoto { get; set; }
    public int? OrderNumber { get; set; }
}