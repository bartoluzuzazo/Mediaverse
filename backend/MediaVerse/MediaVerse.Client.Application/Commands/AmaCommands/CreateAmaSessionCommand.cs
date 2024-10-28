using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.AmaDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.AuthorSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediaVerse.Client.Application.Commands.AmaCommands;

public record CreateAmaSessionCommand : IRequest<BaseResponse<GetAmaSessionResponse>>
{
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public DateTime Start { get; set; }
    [Required]
    public DateTime End { get; set; }
    [Required]
    public Guid AuthorId { get; set; }
}

public class CreateAmaSessionCommandHandler(
    IRepository<Author> authorRepository,
    IUserAccessor userAccessor,
    IRepository<AmaSession> amaSessionRepository,
    IMapper mapper)
    : IRequestHandler<CreateAmaSessionCommand, BaseResponse<GetAmaSessionResponse>>
{
    public async Task<BaseResponse<GetAmaSessionResponse>> Handle(CreateAmaSessionCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Start > request.End)
        {
            return new BaseResponse<GetAmaSessionResponse>(
                new BadRequestException("End date cannot be earlier than start date"));
        }

        var userId = userAccessor.Id;
        if (userId is null)
        {
            return new BaseResponse<GetAmaSessionResponse>(
                new ProblemException("Authorized user should always have an ID"));
        }

        var authorSpec = new GetAuthorWithPhotoSpecification(request.AuthorId);
        var author = await authorRepository.FirstOrDefaultAsync(authorSpec, cancellationToken);
        if (author is null)
        {
            return new BaseResponse<GetAmaSessionResponse>(new NotFoundException("Author not found"));
        }

        if (author.UserId != userId)
        {
            return new BaseResponse<GetAmaSessionResponse>(new ForbiddenException());
        }

        var amaSession = new AmaSession
        {
            Id = Guid.NewGuid(),
            Author = author,
            Start = request.Start,
            End = request.End,
        };
        var addedSession = await amaSessionRepository.AddAsync(amaSession, cancellationToken);
        var amaSessionResponse =mapper.Map<GetAmaSessionResponse>(addedSession);
        return new BaseResponse<GetAmaSessionResponse>(amaSessionResponse);
    }
}