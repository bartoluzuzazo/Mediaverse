namespace MediaVerse.Client.Application.DTOs.EntryDTOs;

public class GetRatedEntryResponse
{
    
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Photo { get; set; } = null!;
    
    public int UsersRating { get; set; }


}