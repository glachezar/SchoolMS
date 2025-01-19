namespace Infrastructure.Identity;

using Application.Exceptions;
using Application.Features.Identity.Users;
using System.Collections.Generic;
using System.Security.Claims;

public class CurrentUserService : ICurrentUserService
{
    private ClaimsPrincipal _principal;

    public string Name => _principal.Identity.Name;

    public IEnumerable<Claim> GetUserClaims()
    {
        return _principal.Claims;
    }

    public string GetUserEmail()
    {
        if (IsAuthenticated())
            return _principal.GetEmail();
        
        return string.Empty;
    }

    public string GetUserId()
    {
        if (IsAuthenticated())
            return _principal.GetUserId();

        return string.Empty;
    }

    public string GetUserTenant()
    {
        if (IsAuthenticated())
            return _principal.GetTenant();

        return string.Empty;
    }

    public bool IsAuthenticated()
    {
        return _principal.Identity.IsAuthenticated;
    }

    public bool IsInRole(string roleName)
    {
        return _principal.IsInRole(roleName);
    }

    public void SetCurrentUser(ClaimsPrincipal principal)
    {
        if (_principal is not null)
        {
            throw new ConflictException("Invalid operation on claim.");
        }

        _principal = principal;
    }

    public void SetCurrentUserId(string userId)
    {
        throw new NotImplementedException();
    }
}
