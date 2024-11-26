using System.Security.Claims;
using MediaVerse.Client.Application.Services.UserAccessor;
using Microsoft.AspNetCore.Http;

namespace MediaVerse.Infrastructure.UserAccessor;

public class HttpUserAccessor(IHttpContextAccessor httpContextAccessor) : IUserAccessor
{
    public string? Email
    {
        get
        {
            return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        }
    }

    public string? Name
    {
        get
        {
            return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        }
    }

    public Guid? Id
    {
        get
        {
            var idString = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return idString is null ? null : Guid.Parse(idString);
        }
        
    }
}