using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinancialSystem.Application.Configuration;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinancialSystem.Application.Services;

public class JwtService(IOptions<AuthSettings> authSettings, ILogger<JwtService> logger) : IJwtService
{
    public string GenerateToken(User user)
    {
        logger.LogInformation("Generating new JWT token for user {UserId}", user.Id);

        try
        {
            var claims = new List<Claim>()
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("LastName", user.LastName),
                new Claim("FirstName", user.FirstName),
                new Claim("email", user.Email),
            };
        
            var expiryDateTime = DateTime.UtcNow.Add(authSettings.Value.Expires);
        
            var secretKeyBytes = Encoding.UTF8.GetBytes(authSettings.Value.SecretKey);

            var jwtToken = new JwtSecurityToken(
                expires: expiryDateTime,
                claims: claims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        secretKeyBytes), SecurityAlgorithms.HmacSha256));
        
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
            logger.LogInformation("JWT token successfully generated for user {UserId}, valid until {ExpiryTime} UTC", 
                user.Id, expiryDateTime);
        
            return tokenString;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error generating JWT token for user {UserId}", user.Id);
            throw;
        }
    }
}