using System.Threading.Tasks;

namespace NotasISPCAN.Features.Service
{
    public interface IRouting
    {
        Task GoBack();
        Task GoBackModal();
        Task NavigateTo(string route);
    }
}
