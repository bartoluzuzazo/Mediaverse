using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Services.Authentication;
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

public class LoginUserQueryHandler(IRepository<User> userRepository, IAuthService authService)
    : IRequestHandler<LoginUserQuery, BaseResponse<UserLoginResponse>>
{
    public async Task<BaseResponse<UserLoginResponse>> Handle(LoginUserQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new GetUserSpecification(request.Email);
        var user = await userRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (user is null) return new BaseResponse<UserLoginResponse>(new NotFoundException());

        var isVerified = authService.VerifyPassword(request.Password, user);

        if (!isVerified) return new BaseResponse<UserLoginResponse>(new NotFoundException());

        var token = authService.CreateToken(user);

        var response = new UserLoginResponse()
        {
            Token = token
        };

        return new BaseResponse<UserLoginResponse>(response);
    }
}