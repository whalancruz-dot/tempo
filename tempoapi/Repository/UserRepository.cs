using Dapper;
using System.Data;
using TempoApi.Models;

namespace TempoApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection cnn;

        public UserRepository(IDbConnection cnn)
        {
            this.cnn = cnn;
        }

        public User Authentication(string email)
        {
            string sql = @"
                SELECT Id, Nome, SenhaHash, Email, DataCriacao 
                FROM Usuarios 
                WHERE Email = @email";

            return cnn.QueryFirstOrDefault<User>(sql, new { email });
        }

        public bool Create(User user)
        {

            string sql = @"
                INSERT INTO Usuarios (Nome, SenhaHash, Email) 
                VALUES (@Nome, @SenhaHash, @Email)";

            int rowsAffected = cnn.Execute(sql, new 
            { 
                user.Nome, 
                user.SenhaHash, 
                user.Email 
            });

            return rowsAffected > 0;
        }

    }
}