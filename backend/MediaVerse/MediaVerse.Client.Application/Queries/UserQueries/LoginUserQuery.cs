using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MediaVerse.Client.Application.Queries.UserQueries;

public record LoginUserQuery : IRequest<BaseResponse<UserLoginResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, BaseResponse<UserLoginResponse>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IConfiguration _configuration;

    public LoginUserQueryHandler(IRepository<User> userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<BaseResponse<UserLoginResponse>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetUserSpecification(request.Email);
        var user = await _userRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (user is null) return new BaseResponse<UserLoginResponse>(new NotFoundException());
        
        var passwordHasher = new PasswordHasher<User>();
        var isVerified = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Success;

        if (!isVerified) return new BaseResponse<UserLoginResponse>(new NotFoundException());

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var claims = new List<Claim>()
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Username),
            new (ClaimTypes.Email, user.Email),
        };

        user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)).ToList().ForEach(claim => claims.Add(claim));
        
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)),
                SecurityAlgorithms.HmacSha256
            )
            
        };
        var token = tokenHandler.CreateToken(tokenDescription);

        var response = new UserLoginResponse()
        {
            Token = tokenHandler.WriteToken(token)
        };

        return new BaseResponse<UserLoginResponse>(response);
    }
}