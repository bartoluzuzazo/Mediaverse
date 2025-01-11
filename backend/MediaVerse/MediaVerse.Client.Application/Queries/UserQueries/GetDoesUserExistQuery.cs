using MediatR;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.UserQueries;

public record GetDoesUserExistQuery(string Email) : IRequest<BaseResponse<ExistenceCheckResult>>;

public class GetDoesUserExistQueryHandler(IRepository<User> userRepository) : IRequestHandler<GetDoesUserExistQuery, BaseResponse<ExistenceCheckResult>>
{
    public async Task<BaseResponse<ExistenceCheckResult>> Handle(GetDoesUserExistQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetUserByEmailWithPictureSpecification(request.Email);
        var user = await userRepository.FirstOrDefaultAsync(spec, cancellationToken);
        return new BaseResponse<ExistenceCheckResult>(new ExistenceCheckResult
        {
            Exists = user is not null,
        });
    }
}