using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Services.UserAccessor;

public interface IUserService
{
   public  Task<BaseResponse<User>> GetCurrentUserAsync(CancellationToken cancellationToken);
}