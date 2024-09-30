using MediatR;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Services.Authentication;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.UserCommands;

public class UpdateUserCommand : IRequest<BaseResponse<UserLoginResponse>>
{
    public string? Email { get; set; }
    public string? ProfilePicture { get; set; }
}

public class UpdateUserCommandHandler(IUserService userService, IRepository<User> userRepository, IRepository<ProfilePicture> profilePictureRepository, IAuthService authService) : IRequestHandler<UpdateUserCommand, BaseResponse<UserLoginResponse>>
{
    public async Task<BaseResponse<UserLoginResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUserResp = await userService.GetCurrentUserAsync(cancellationToken);
        if (currentUserResp.Exception is not null)
        {
            return new BaseResponse<UserLoginResponse>(currentUserResp.Exception);
        }

        var user = currentUserResp.Data!;
        user.Email = request.Email ?? user.Email;
        
        if (request.ProfilePicture is not null)
        {
            var photoData = Convert.FromBase64String(request.ProfilePicture);

            var profilePicture = new ProfilePicture()
            {
                Id = Guid.NewGuid(),
                Picture = photoData
            };
            var newPicture = await profilePictureRepository.AddAsync(profilePicture, cancellationToken);
            user.ProfilePicture = newPicture;
        }

        var saveChanges = await userRepository.SaveChangesAsync(cancellationToken);
        var token = authService.CreateToken(user);
        var response = new UserLoginResponse()
        {
            Token = token
        };
        return new BaseResponse<UserLoginResponse>(response);
    }
}
    