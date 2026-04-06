

using TempoApi.Models;

namespace TempoApi.Repository
{
    public interface IUserRepository
    {
        User Authentication(string email);
        bool Create(User user);
    }
}
