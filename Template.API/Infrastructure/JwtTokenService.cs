using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Template.API.Infrastructure;

/// <summary>
///     Jwt Token Service
/// </summary>
public static class JwtTokenService
{
    /// <summary>
    ///     Generate Jwt Token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GenerateJwtToken(string userId, string name)
    {
        var securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Builder.ConfigurationReader!.GetJwtSettingsKey()!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Name, name ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(Builder.ConfigurationReader.GetJwtSettingsIssuer(),
            Builder.ConfigurationReader.GetJwtSettingsAudience(),
            claims,
            expires: DateTime.Now.AddYears(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}