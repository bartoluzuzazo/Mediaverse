namespace MediaVerse.Client.Application.DTOs.ReviewDtos;

public class GetReviewSummaryResponse
{
    public decimal GradeAvg { get; set; }

    public List<GetReviewPreviewResponse> Reviews { get; set; } = null!;
}