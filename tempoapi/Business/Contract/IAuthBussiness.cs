

using TempoApi.Models;

namespace TempoApi.Business.Contract
{
    public interface IAuthBussiness
    {
        Result<string> Authentication(UserLoginRequest parametros);
    }
}

