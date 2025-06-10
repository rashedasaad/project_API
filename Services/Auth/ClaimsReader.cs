using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace project_API.Services.Auth;

[Authorize]
public class ClaimsReader 
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimsReader(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetByClaimType(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    }


    
    
}