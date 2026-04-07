
using TempoApi.Models;

namespace TempoApi.Business.Contract
{
    public interface IFavoriteBussiness
    {
        public string Salvar(Favorite parametro);
        public string Remover(Guid id);
        IEnumerable<Favorite> Buscar(Guid usuarioid);
    }
}

