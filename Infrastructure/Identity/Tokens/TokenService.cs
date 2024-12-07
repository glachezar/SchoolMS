namespace Infrastructure.Identity.Tokens;

using Application.Features.Identity.Tokens;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class TokenService(UserManager<ApplicationUser> userManager, SchoolTenantInfo tenant) : ITokenService
{
    readonly UserManager<ApplicationUser> _userManager = userManager;
    readonly SchoolTenantInfo _tenant = tenant;

    public async Task<TokenResponse> LoginAsync(TokenRequest request)
    {
        var userInDb = await _userManager.FindByEmailAsync(request.Email);
        if (userInDb is null)
        {
            throw new Exception("Invalid email or password");
        }

        if (!await _userManager.CheckPasswordAsync(userInDb, request.Password))
        {
            throw new Exception("Invalid email or password");
        }

        if (!userInDb.IsActive)
        {
            throw new Exception("User is not active");
        }

        if (_tenant.Id != TenancyConstants.Root.Id)
        {
            if (_tenant.ValidUpTo < DateTime.UtcNow)
            {
                throw new Exception("Tenant subscription expired!");
            }
        }

    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        throw new NotImplementedException();
    }

    private async Task<TokenResponse> GenerateTokenAndUpdateUserAsync(ApplicationUser user)
    {
        var newToken = GenerateJwt(user);
        user.RefreshToken = GenerateRefreshToken();
    }

    private string GenerateJwt(ApplicationUser user)
    {
        return GenerateEncryptedToken(GetSigningCredentials(), GetUserClaims(user));
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token =
            new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes("superSecret2345678");
        var secretKey = new SymmetricSecurityKey(secret);
        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }

    private IEnumerable<Claim> GetUserClaims(ApplicationUser user)
    {
        return
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim(ClaimConstants.Tenant, _tenant.Id),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
        ];
    }

    private string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }
}
