using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Infrastructure.UserAccessor;

public class HttpUserService : IUserService
{
    
    private readonly IUserAccessor _userAccessor;
    private readonly IRepository<User> _userRepository;

    public HttpUserService(IUserAccessor userAccessor, IRepository<User> userRepository)
    {
        _userAccessor = userAccessor;
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<User>> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        
        var email = _userAccessor.Email;
        if (email is null)
        {
            return new BaseResponse<User>(new ProblemException());
        }

        var user = await _userRepository.FirstOrDefaultAsync(new GetUserSpecification(email), cancellationToken);
        if (user is null)
        {
            return new BaseResponse<User>(new NotFoundException());
        }

        return new BaseResponse<User>(user);
    }
    
}