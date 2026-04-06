
using TempoApi.Business.Contract;
using TempoApi.Models;
using TempoApi.Repository;


namespace TempoApi.Business
{
    public class UserBussiness : IUserBussiness
    {
        private readonly IUserRepository _userRepository;

        public UserBussiness(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Result<bool> CreateUser(UserSiginRequest request)
        {
            var usuarioExistente = _userRepository.Authentication(request.Email);
            if (usuarioExistente != null)  return Result<bool>.Failure("Usuário já cadastrado.");

            string senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var parametros = new User
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = senhaCriptografada
            };

            _userRepository.Create(parametros);

            return Result<bool>.Ok(true, "Usuário regiustrado com sucesso!");
        }

    }
}