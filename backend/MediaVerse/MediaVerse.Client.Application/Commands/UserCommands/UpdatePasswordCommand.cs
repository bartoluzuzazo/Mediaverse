using MediatR;
using MediaVerse.Client.Application.Services.Authentication;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace MediaVerse.Client.Application.Commands.UserCommands;

public record UpdatePasswordCommand : IRequest<Exception?>
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

public class UpdatePasswordCommandHandler(IUserService userService, IAuthService authService)
    : IRequestHandler<UpdatePasswordCommand, Exception?>
{
    public async Task<Exception?> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var userResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (userResp.Exception is not null)
        {
            return userResp.Exception;
        }

        var user = userResp.Data!;

        var isVerified = authService.VerifyPassword(request.OldPassword, user);
        if (isVerified)
        {
            return new NotAuthorizedException("Bad password");
        }
        var passwordHasher = new PasswordHasher<User>();
        var hashedPassword = passwordHasher.HashPassword(new User(), request.NewPassword);
        user.PasswordHash = hashedPassword;
        return null;
    }
}