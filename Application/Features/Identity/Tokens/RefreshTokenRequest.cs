namespace Application.Features.Identity.Tokens;

public class RefreshTokenRequest
{
    public string CurrentJwtToken { get; set; }

    public string CurrentRefreshToken { get; set; }
}
