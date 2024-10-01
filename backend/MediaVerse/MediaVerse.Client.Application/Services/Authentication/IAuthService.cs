using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Services.Authentication;

public interface IAuthService
{
    string CreateToken(User user);
    bool VerifyPassword(string providedPassword, User user);
}