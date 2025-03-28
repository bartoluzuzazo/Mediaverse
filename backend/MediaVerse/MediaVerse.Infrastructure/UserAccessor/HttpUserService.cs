using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Infrastructure.UserAccessor;

public class HttpUserService(IUserAccessor userAccessor, IRepository<User> userRepository)
    : IUserService
{
    public async Task<BaseResponse<User>> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        
        var email = userAccessor.Email;
        if (email is null)
        {
            return new BaseResponse<User>(new ProblemException());
        }

        var user = await userRepository.FirstOrDefaultAsync(new GetUserSpecification(email), cancellationToken);
        if (user is null)
        {
            return new BaseResponse<User>(new NotFoundException());
        }

        return new BaseResponse<User>(user);
    }

    public async Task<BaseResponse<User>> GetCurrentUserWithPictureAsync(CancellationToken cancellationToken)
    {
        var email = userAccessor.Email;
        if (email is null)
        {
            return new BaseResponse<User>(new ProblemException());
        }
        var spec = new GetUserByEmailWithPictureSpecification(email);
        var user = await userRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (user is null)
        {
            return new BaseResponse<User>(new NotFoundException());
        }

        return new BaseResponse<User>(user);
    }
}