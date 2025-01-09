namespace Infrastructure.Identity.Auth;

using Microsoft.AspNetCore.Authorization;

internal class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; set; } = permission;
}
