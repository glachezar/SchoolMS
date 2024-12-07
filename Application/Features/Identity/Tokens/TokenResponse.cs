namespace Application.Features.Identity.Tokens;

public class TokenResponse
{
    public string JwtToken { get; set; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryDate { get; set; }
}
