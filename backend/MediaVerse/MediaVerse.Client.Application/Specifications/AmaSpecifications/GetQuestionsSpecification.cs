
using Ardalis.Specification;
using MediaVerse.Client.Application.DTOs.AmaDTOs;
using MediaVerse.Client.Application.Extensions.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Specifications.AmaSpecifications;

public class GetQuestionsSpecification : Specification<AmaQuestion, GetAmaQuestionResponse>
{
    public GetQuestionsSpecification(Guid sessionId, Guid? userId, int page, int size, QuestionOrder order, OrderDirection direction ,QuestionStatus status)
    {
        Query.Where(aq => aq.AmaSessionId == sessionId);
        switch (status)
        {
            case QuestionStatus.Answered:
                Query.Where(aq => aq.Answer != null);
                break;
            case QuestionStatus.Unanswered:
                Query.Where(aq => aq.Answer == null);
                break;
        }
        switch (order)
        {
            case QuestionOrder.TotalVotes:
                Query.OrderByDirection(aq=> aq.Users.Count(), direction);
                break;
        }

        Query.Select(aq =>
            new GetAmaQuestionResponse
            {
                Id = aq.Id,
                AmaSessionId = sessionId,
                Answer = aq.Answer,
                Content = aq.Content,
                Likes = aq.Users.Count,
                Username = aq.User.Username,
                ProfilePicture = aq.User.ProfilePicture.Picture != null ?
                    Convert.ToBase64String(aq.User.ProfilePicture.Picture): null,
                LikedByUser = userId != null && aq.Users.Any(u=>u.Id == userId),
            });
        
        Query.Paginate(page, size);
        
    }
}