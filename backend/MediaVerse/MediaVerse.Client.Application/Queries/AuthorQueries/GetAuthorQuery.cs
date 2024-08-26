using MediatR;
using MediaVerse.Client.Application.DTOs.AuthorDTOs;
using MediaVerse.Client.Application.Specifications.AuthorSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MediaVerse.Client.Application.Queries.AuthorQueries;

public record GetAuthorQuery : IRequest<BaseResponse<GetAuthorResponse>>
{
    public Guid Id { get; set; }
}

public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, BaseResponse<GetAuthorResponse>>
{
    private readonly IRepository<Author> _authorRepository;

    public GetAuthorQueryHandler(IRepository<Author> authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<BaseResponse<GetAuthorResponse>> Handle(GetAuthorQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new GetAuthorWithPhotoSpecification(request.Id);
        var author = await _authorRepository.FirstOrDefaultAsync(specification, cancellationToken);
        if (author is null)
        {
            return new BaseResponse<GetAuthorResponse>(new NotFoundException());
        }


        var authorResponse = new GetAuthorResponse()
        {
            Id = author.Id,
            Bio = author.Bio,
            Name = author.Name,
            Surname = author.Surname,
            ProfilePicture = Convert.ToBase64String(author.ProfilePicture.Picture),
        };
        return new BaseResponse<GetAuthorResponse>(authorResponse);
    }
}