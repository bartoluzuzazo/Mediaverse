using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Services.Authentication;

public interface ITokenService
{
    string CreateToken(User user);
}