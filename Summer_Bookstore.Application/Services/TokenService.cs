using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Summer_Bookstore.Application.Services;
using Summer_Bookstore.Application.Settings;
using Summer_Bookstore_Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string CreateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /*
        This method creates a **JWT token** for a given user.

        1. It first defines the **claims** (username and role) that go inside the token.
        2. Then, it builds a **security key** using secret, and signs the token with **HMAC SHA-256**.
        3. Finally, it creates the token with issuer, audience, expiry, and returns it as a string.

        This token can now be sent to clients and used for authenticated API access.
     */

}
