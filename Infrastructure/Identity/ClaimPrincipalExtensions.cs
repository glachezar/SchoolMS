﻿namespace Infrastructure.Identity;

using Infrastructure.Identity.Constants;
using System.Security.Claims;

public static class ClaimPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal principal) => 
        principal.FindFirstValue(ClaimTypes.Email);
    
    public static string GetUserId(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier);

    public static string GetFirstName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Name);

    public static string GetLastName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Surname);

    public static string GetTenant(this ClaimsPrincipal principal) =>
    principal.FindFirstValue(ClaimConstants.Tenant);

    public static string GetPhoneNumber(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.MobilePhone);
}
