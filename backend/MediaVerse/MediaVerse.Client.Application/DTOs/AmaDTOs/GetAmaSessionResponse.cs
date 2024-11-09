
namespace MediaVerse.Client.Application.DTOs.AmaDTOs;

public class GetAmaSessionResponse
{
  public Guid Id { get; set; }

  public DateTime Start { get; set; }

  public DateTime End { get; set; }
  public string AuthorName { get; set; }
  public string AuthorSurname { get; set; }
  public string ProfilePicture { get; set; }
  public string Title { get; set; }
  public string Description { get; set; }
  public Guid AuthorUserId { get; set; }

}
