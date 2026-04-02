namespace TempoApi.Models
{

    public class Favorite
    {
        public Guid Id { get; set; } 
        public int CidadeId { get; set; }
        public string Nome { get; set; }
        public string State { get; set; }
        public string UsuarioId { get; set; }
    }


}
