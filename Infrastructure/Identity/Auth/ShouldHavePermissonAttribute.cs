namespace Infrastructure.Identity.Auth;

using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Authorization;

public class ShouldHavePermissonAttribute : AuthorizeAttribute
{
    public ShouldHavePermissonAttribute( string action, string feature)
    {
        Policy = SchoolPermission.NameFor(action, feature);
    }
}
