using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Domain.Specifications.UserSpecifications;
using Microsoft.AspNetCore.Identity;

namespace MediaVerse.Client.Application.Commands.UserCommands;

public record RegisterUserCommand : IRequest<CommandBaseResponse<UserRegisterResponse>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, CommandBaseResponse<UserRegisterResponse>>
{
    private readonly IRepository<User> _userRepository;

    public RegisterUserCommandHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<CommandBaseResponse<UserRegisterResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHasher = new PasswordHasher<User>();
        var hashedPassword = passwordHasher.HashPassword(new User(), request.Password);

        var spec = new UserExistsSpec(request.Username, request.Email);
        var userExists = await _userRepository.AnyAsync(spec, cancellationToken);

        if (userExists) return new CommandBaseResponse<UserRegisterResponse>(new ConflictException("User already exists"));
        
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = hashedPassword,
            ProfilePictureId = new Guid("dbc2478f-ee4f-492e-afdd-7584a81e2baa") //temporary picture placeholder, TODO: add profile picture field
        };
        
        await _userRepository.AddAsync(user, cancellationToken);
        var response = new UserRegisterResponse()
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            PictureId = user.ProfilePictureId
        };
        return new CommandBaseResponse<UserRegisterResponse>(response);
    }
}