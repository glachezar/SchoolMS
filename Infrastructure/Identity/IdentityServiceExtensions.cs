namespace Infrastructure.Identity;

using Application.Features.Identity.Roles;
using Application.Features.Identity.Tokens;
using Application.Features.Identity.Users;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Tokens;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

internal static class IdentityServiceExtensions
{
    internal static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        return services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .Services
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IRoleService, RoleService>()
            .AddTransient<IUserService, UserService>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<CurrentUserMiddleware>();
    }

    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CurrentUserMiddleware>();
    }

    internal static IServiceCollection AddPermissions(this IServiceCollection services)
    {
        return services
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }
}
