using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Specifications.AuthorSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.AuthorQueries;

public record GetLinkedUserQuery(Guid AuthorId) : IRequest<BaseResponse<GetUserResponse>>;

public class GetLinkedUserQueryHandler(IMapper mapper, IRepository<Author> authorRepository) : IRequestHandler<GetLinkedUserQuery, BaseResponse<GetUserResponse>>
{
    public async Task<BaseResponse<GetUserResponse>> Handle(GetLinkedUserQuery request, CancellationToken cancellationToken)
    {
        var getAuthorWithUserSpecification = new GetAuthorWithUserSpecification(request.AuthorId);
        var author = await authorRepository.FirstOrDefaultAsync(getAuthorWithUserSpecification,cancellationToken);
        if (author is null)
        {
            return new BaseResponse<GetUserResponse>(new NotFoundException());
        }
        var response = mapper.Map<GetUserResponse>(author.User);
        response.Authors = null;
        return new BaseResponse<GetUserResponse>(response);
    }
}