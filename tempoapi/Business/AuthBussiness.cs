using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TempoApi.Business.Contract;


namespace TempoApi.Business
{
    public class AuthBussiness : IAuthBussiness
    {

        private readonly IConfiguration _config;

        public AuthBussiness(IConfiguration config) => _config = config;

        public string GerarToken(string uuid)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]  {
            new Claim(ClaimTypes.NameIdentifier, uuid),
            new Claim("device_uuid", uuid),
            new Claim(ClaimTypes.Role, "user")
        };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}