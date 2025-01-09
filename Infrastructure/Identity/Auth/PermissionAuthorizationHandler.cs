namespace Infrastructure.Identity.Auth;

using Application.Features.Identity.Users;
using Microsoft.AspNetCore.Authorization;

internal class PermissionAuthorizationHandler(IUserService userService) 
    : AuthorizationHandler<PermissionRequirement>
{
    readonly IUserService _userService = userService;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.GetUserId() is { } userId && 
            await _userService.IsPermissionAssignedAsync(userId, requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}