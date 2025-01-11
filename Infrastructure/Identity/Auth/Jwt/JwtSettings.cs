namespace Infrastructure.Identity.Auth.Jwt;

public class JwtSettings
{
    public string Key { get; set; }
    public int TokenExpiaryTimeInMinutes { get; set; }
    public int RefreshTokenExpiaryTimeInDays { get; set; }
}
