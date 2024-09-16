using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediaVerse.Client.Application.Services.Authentication;
using MediaVerse.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Infrastructure.Authentication;

public class TokenService(IConfiguration configuration) : ITokenService
{
    
    public string CreateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
        };

        user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)).ToList().ForEach(claim => claims.Add(claim));

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"],
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                SecurityAlgorithms.HmacSha256
            )
        };
        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }
}