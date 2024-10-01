namespace MediaVerse.Client.Application.DTOs.RatingDTOs;

public class PutRatingDto
{
    public int Grade { get; set; }
    public Guid EntryId { get; set; }
}