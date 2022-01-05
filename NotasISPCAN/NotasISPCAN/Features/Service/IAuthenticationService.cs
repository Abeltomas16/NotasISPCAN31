using System.Threading.Tasks;

namespace NotasISPCAN.Features.Service
{
    public interface IAuthenticationService
    {
        Task<bool> IsLogged();
    }
}
