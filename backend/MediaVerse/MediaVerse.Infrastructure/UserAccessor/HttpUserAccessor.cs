using System.Security.Claims;
using MediaVerse.Client.Application.Services.UserAccessor;
using Microsoft.AspNetCore.Http;

namespace MediaVerse.Infrastructure.UserAccessor;

public class HttpUserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public string? Email
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        }
    }

    public string? Name
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}