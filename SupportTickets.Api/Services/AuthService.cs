using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SupportTickets.Api.Services;

public class AuthService : IAuthService
{
    private readonly string _key;
    private readonly string _issuer;

public AuthService(IConfiguration config)
{
        _key = config["Jwt:Key"] ?? "ThisIsAReallyStrongJwtDevKey_123456";
        _issuer = config["Jwt:Issuer"] ?? "SmartTickets";
}

public string? Login(string username, string password)
{
    // Admin login
    if (username == "admin" && password == "admin123")
    {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("role", "Admin") 
            };

        return GenerateToken(claims);
    }

    return null; // invalid login
}

public string GuestLogin()
{
    var claims = new[]
    {
        new Claim("role", "Guest")
    };

    return GenerateToken(claims);
}


private string GenerateToken(IEnumerable<Claim> claims)
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _issuer,
        audience: _issuer,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

}
