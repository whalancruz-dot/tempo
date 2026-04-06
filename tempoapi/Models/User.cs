namespace TempoApi.Models
{

    public class User
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string SenhaHash { get; set; }
        public string Email { get; set; }
    }

}
