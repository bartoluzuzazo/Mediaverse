
namespace MediaVerse.Client.Application.DTOs.AmaDTOs;

public class GetAmaQuestionResponse
{
  public Guid Id { get; set; }

  public Guid AmaSessionId { get; set; }

  public Guid UserId { get; set; }
  public string Username { get; set; } = null!;
  public string? ProfilePicture { get; set; }

  public string Content { get; set; } = null!;

  public string? Answer { get; set; }
  public int Likes { get; set; }
  public bool LikedByUser { get; set; }
}
