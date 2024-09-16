using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.UserQueries;

public record GetUserByEmailQuery(string Email) : IRequest<BaseResponse<GetUserResponse>>;

public class GetUserByEmailQueryHandler(IRepository<User> userRepository, IMapper mapper)
    : IRequestHandler<GetUserByEmailQuery, BaseResponse<GetUserResponse>>
{
    public async Task<BaseResponse<GetUserResponse>> Handle(GetUserByEmailQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new GetUserByEmailWithPictureSpecification(request.Email);
        var user = await userRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (user is null)
        {
            return new BaseResponse<GetUserResponse>(new NotFoundException());
        }

        var userResponse = mapper.Map<GetUserResponse>(user);
        return new BaseResponse<GetUserResponse>(userResponse);
    }
}