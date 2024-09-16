using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.UserQueries;

public record GetUserQuery(Guid UserId) : IRequest<BaseResponse<GetUserResponse>>;

public class GetUserQueryHandler(IRepository<User> userRepository, IMapper mapper)
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

        return new BaseResponse<GetUserResponse>(userResponse);
    }
}