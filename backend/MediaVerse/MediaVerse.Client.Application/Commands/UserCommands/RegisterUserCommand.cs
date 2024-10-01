using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Specifications.RoleSpecifications;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MediaVerse.Client.Application.Commands.UserCommands;

public record RegisterUserCommand : IRequest<BaseResponse<UserRegisterResponse>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterUserCommandHandler(IRepository<User> userRepository, IRepository<Role> roleRepository)
    : IRequestHandler<RegisterUserCommand, BaseResponse<UserRegisterResponse>>
{
    public async Task<BaseResponse<UserRegisterResponse>> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var passwordHasher = new PasswordHasher<User>();
        var hashedPassword = passwordHasher.HashPassword(new User(), request.Password);

        var usernameSpec = new GetUserByUsernameSpecification(request.Username);
        var isUsernameTaken = await userRepository.AnyAsync(usernameSpec, cancellationToken);
        if (isUsernameTaken)
        {
            return new BaseResponse<UserRegisterResponse>(
                new ConflictException("User with this username already exists"));
        }

        var emailSpec = new GetUserByEmailSpecification(request.Email);
        var isEmailTaken = await userRepository.AnyAsync(emailSpec, cancellationToken);
        if (isEmailTaken)
        {
            return new BaseResponse<UserRegisterResponse>(new ConflictException("User with this email already exists"));
        }


        var roleSpec = new GetUserRoleSpecification();
        var userRole = await roleRepository.FirstOrDefaultAsync(roleSpec, cancellationToken);

        if (userRole is null) return new BaseResponse<UserRegisterResponse>(new Exception("User role does not exist"));

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = hashedPassword,
            ProfilePictureId =
                new Guid(
                    "dbc2478f-ee4f-492e-afdd-7584a81e2baa"), //temporary picture placeholder, TODO: add profile picture field
            Roles = new List<Role> { userRole }
        };

        await userRepository.AddAsync(user, cancellationToken);

        var response = new UserRegisterResponse()
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            PictureId = user.ProfilePictureId
        };
        return new BaseResponse<UserRegisterResponse>(response);
    }
}