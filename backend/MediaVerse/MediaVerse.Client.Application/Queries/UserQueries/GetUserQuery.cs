using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.UserQueries;

public record GetUserQuery(Guid UserId) : IRequest<BaseResponse<GetUserResponse>>;

public class GetUserQueryHandler(IRepository<User> userRepository, IRepository<Author> authorRepository, IMapper mapper)
    : IRequestHandler<GetUserQuery, BaseResponse<GetUserResponse>>
{
    public async  Task<BaseResponse<GetUserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetUserByIdWithPictureSpecification(request.UserId);
        var user = await userRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (user is null)
        {
            return new BaseResponse<GetUserResponse>(new NotFoundException());
        }

        var userResponse = mapper.Map<GetUserResponse>(user);
        
        var authorSpec = new GetLinkedAuthorSpecification(request.UserId);
        var response = await authorRepository.ListAsync(authorSpec, cancellationToken);

        if (!response.IsNullOrEmpty()) userResponse.Authors = response;

        return new BaseResponse<GetUserResponse>(userResponse);
    }
}