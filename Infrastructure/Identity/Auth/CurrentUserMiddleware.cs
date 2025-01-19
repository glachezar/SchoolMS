namespace Infrastructure.Identity.Auth;

using Application.Features.Identity.Users;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class CurrentUserMiddleware(ICurrentUserService currentUserService) : IMiddleware
{
    readonly ICurrentUserService _currentUserService = currentUserService;
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _currentUserService.SetCurrentUser(context.User);
        await next(context);
    }
}