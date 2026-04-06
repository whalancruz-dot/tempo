using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TempoApi.Business.Contract;
using TempoApi.Models;
using TempoApi.Repository;


namespace TempoApi.Business
{
    public class AuthBussiness : IAuthBussiness
    {

        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthBussiness(IConfiguration config, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public Result<string> Authentication(UserLoginRequest parametros)
        {
            var usuarioExistente = _userRepository.Authentication(parametros.Email);
            if (usuarioExistente == null) return Result<string>.Failure("Usuário não disponivel.");

            bool senhaValida = BCrypt.Net.BCrypt.Verify(parametros.Password, usuarioExistente.SenhaHash);
            if (!senhaValida) return Result<string>.Failure("Senha incorreta.");

            var token = GerarToken(usuarioExistente.Id.ToString(), usuarioExistente.Nome);

            return Result<string>.Ok(token, "Login efetuado com sucesso!");
        }

        public string GerarToken(string uuid, string nome) // Adicionado parâmetro 'nome'
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
        new Claim(ClaimTypes.NameIdentifier, uuid),
        new Claim("name", nome),
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