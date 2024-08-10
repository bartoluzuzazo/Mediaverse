using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Domain.Specifications.RoleSpecifications;
using MediaVerse.Domain.Specifications.UserSpecifications;
using Microsoft.AspNetCore.Identity;

namespace MediaVerse.Client.Application.Commands.UserCommands;

public record RegisterUserCommand : IRequest<BaseResponse<UserRegisterResponse>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, BaseResponse<UserRegisterResponse>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;

    public RegisterUserCommandHandler(IRepository<User> userRepository, IRepository<Role> roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }
    
    public async Task<BaseResponse<UserRegisterResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHasher = new PasswordHasher<User>();
        var hashedPassword = passwordHasher.HashPassword(new User(), request.Password);

        var existsSpec = new UserExistsSpec(request.Username, request.Email);
        var userExists = await _userRepository.AnyAsync(existsSpec, cancellationToken);

        if (userExists) return new BaseResponse<UserRegisterResponse>(new ConflictException("User already exists"));

        var roleSpec = new GetUserRoleSpecification();
        var userRole = await _roleRepository.FirstOrDefaultAsync(roleSpec, cancellationToken);

        if (userRole is null) return new BaseResponse<UserRegisterResponse>(new Exception("User role does not exist"));
         
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = hashedPassword,
            ProfilePictureId = new Guid("dbc2478f-ee4f-492e-afdd-7584a81e2baa"), //temporary picture placeholder, TODO: add profile picture field
            Roles = new List<Role>{userRole}
        };
        
        await _userRepository.AddAsync(user, cancellationToken);
        
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