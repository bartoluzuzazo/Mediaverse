namespace MediaVerse.Client.Application.DTOs.EntryDTOs.RatingDTOs;

public class GetRatingResponse
{
    public Guid Id { get; set; }
    public int Grade { get; set; }
    public Guid UserId { get; set; }
    public Guid EntryId { get; set; }
}