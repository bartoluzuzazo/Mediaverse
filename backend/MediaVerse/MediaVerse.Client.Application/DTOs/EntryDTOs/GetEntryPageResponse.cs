namespace MediaVerse.Client.Application.DTOs.EntryDTOs;

public class GetEntryPageResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Photo { get; set; }
    public decimal RatingAvg { get; set; }
}