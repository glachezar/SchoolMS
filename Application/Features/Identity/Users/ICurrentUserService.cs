namespace Application.Features.Identity.Users;

using System.Security.Claims;

public interface ICurrentUserService
{
    public string Name { get; }
    public string GetUserId();
    public string GetUserEmail();
    public string GetUserTenant();
    public bool IsAuthenticated();
    public bool IsInRole(string roleName);
    public void SetCurrentUser(ClaimsPrincipal principal);
    public void SetCurrentUserId(string userId);
    public IEnumerable<Claim> GetUserClaims();
}
