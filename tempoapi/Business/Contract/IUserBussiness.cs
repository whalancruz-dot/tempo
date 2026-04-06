

using TempoApi.Models;

namespace TempoApi.Business.Contract
{
    public interface IUserBussiness
    {
        Result<bool> CreateUser(UserSiginRequest request);
    }
}

