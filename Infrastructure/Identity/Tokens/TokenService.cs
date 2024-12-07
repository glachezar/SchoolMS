namespace Infrastructure.Identity.Tokens;

using Application.Exceptions;
using Application.Features.Identity.Tokens;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            throw new UnauthorizedException("Authentication not successful!");
        }

        if (!await _userManager.CheckPasswordAsync(userInDb, request.Password))
        {
            throw new UnauthorizedException("Incorrect Username or Password!");
        }

        if (!userInDb.IsActive)
        {
            throw new UnauthorizedException("User Not Active. Please contact administrator.");
        }

        if (_tenant.Id != TenancyConstants.Root.Id)
        {
            if (_tenant.ValidUpTo < DateTime.UtcNow)
            {
                throw new UnauthorizedException("Tenant subscription has expired. Please contact administrator.");
            }
        }

        return await GenerateTokenAndUpdateUserAsync(userInDb);
    }

    public Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        throw new NotImplementedException();
    }

    private async Task<TokenResponse> GenerateTokenAndUpdateUserAsync(ApplicationUser user)
    {
        var newToken = GenerateJwt(user);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);
        return new() 
        {
            JwtToken = newToken,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiryDate = user.RefreshTokenExpiryTime
        };
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
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
