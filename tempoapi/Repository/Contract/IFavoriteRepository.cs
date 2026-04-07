

using TempoApi.Models;

namespace TempoApi.Repository
{
    public interface IFavoriteRepository
    {
        string Salvar(Favorite parametro);
        string Remover(Guid id);
        IEnumerable<Favorite> Buscar(Guid usuarioid);
    }
}
