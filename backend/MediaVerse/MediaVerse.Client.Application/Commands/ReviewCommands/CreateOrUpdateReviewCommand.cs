using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.ReviewDtos;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.ReviewSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.ReviewCommands;

public record CreateOrUpdateReviewCommand(Guid EntryId, CreateUpdateReviewDto Dto) : IRequest<BaseResponse<GetReviewResponse>>;

public class CreateReviewCommandHandler(IUserService userService, IRepository<Entry> entryRepository, IRepository<Review> reviewRepository, IMapper mapper) : IRequestHandler<CreateOrUpdateReviewCommand, BaseResponse<GetReviewResponse>>
{
    public async Task<BaseResponse<GetReviewResponse>> Handle(CreateOrUpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserWithPictureAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return new BaseResponse<GetReviewResponse>(userResp.Exception);
        }
        var user = userResp.Data!;

        var entry = await entryRepository.GetByIdAsync(request.EntryId, cancellationToken);
        if (entry is null)
        {
            return new BaseResponse<GetReviewResponse>(new NotFoundException("Entry not found"));
        }
        var specification = new GetReviewByIdsSpecification(user.Id,entry.Id);
        var existingReview = await reviewRepository.FirstOrDefaultAsync(specification, cancellationToken);
        if (existingReview is not null)
        {
            existingReview.Content = request.Dto.Content;
            existingReview.Grade = request.Dto.Grade;
            existingReview.Title = request.Dto.Title;
            await reviewRepository.UpdateAsync(existingReview, cancellationToken);
            
            var existingReviewResponse = mapper.Map<GetReviewResponse>(existingReview);
            return new BaseResponse<GetReviewResponse>(existingReviewResponse);
            
        }

        var review = new Review
        {
            User = user,
            Entry = entry,
            Content = request.Dto.Content,
            Title = request.Dto.Title,
            Grade = request.Dto.Grade
        };
        await reviewRepository.AddAsync(review, cancellationToken);

        var getReviewResponse = mapper.Map<GetReviewResponse>(review);
        return new BaseResponse<GetReviewResponse>(getReviewResponse);
    }
}