using TempoApi.Business.Contract;
using TempoApi.Models;
using TempoApi.Repository;

namespace TempoApi.Business
{
    public class FavoriteBussiness : IFavoriteBussiness
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteBussiness(IFavoriteRepository favoriteRepository)
        {
            this._favoriteRepository = favoriteRepository;
        }

        public string Remover(Guid id)
        {
            return _favoriteRepository.Remover(id);
        }

        public string Salvar(Favorite parametro)
        {
            return _favoriteRepository.Salvar(parametro);
        }

        public IEnumerable<Favorite> Buscar(Guid usuarioid)
        {
            return _favoriteRepository.Buscar(usuarioid);
        }

    }
}