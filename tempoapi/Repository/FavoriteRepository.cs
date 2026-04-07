using Dapper;
using System.Data;
using TempoApi.Models;

namespace TempoApi.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly IDbConnection cnn;

        public FavoriteRepository(IDbConnection cnn)
        {
            this.cnn = cnn;
        }

        public string Remover(Guid id)
        {
            string sql = "DELETE FROM CidadesFavoritas WHERE id = @id";

            int linhasAfetadas = cnn.Execute(sql, new { id });

            if (linhasAfetadas > 0) return "Cidade removida dos favoritos!";

            return "Cidade não encontrada ou já removida.";
        }

        public string Salvar(Favorite parametro)
        {
            string sql = @"
                 INSERT INTO CidadesFavoritas (cidadeid, nome, usuarioId, state)
                 VALUES (@CidadeId, @Nome, @UsuarioId, @State)";

            cnn.Execute(sql, parametro);

            return "Cidade favoritada com sucesso!";
        }

        public IEnumerable<Favorite> Buscar(Guid usuarioid)
        {
            string sql = @"
            SELECT id, cidadeid as CidadeId, nome, usuarioId, state 
            FROM CidadesFavoritas 
            WHERE usuarioId = @usuarioid";

            var resultados = cnn.Query<Favorite>(sql, new { usuarioid });

            return resultados;
        }

    }
}
