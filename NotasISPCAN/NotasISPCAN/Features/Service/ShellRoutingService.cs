using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotasISPCAN.Features.Service
{
    public class IRoutingService : IRouting
    {
        public IRoutingService()
        {
        }
        public Task NavigateTo(string route)
        {
            return Shell.Current.GoToAsync(route);
        }
        public Task GoBack()
        {
            return Shell.Current.Navigation.PopAsync();
        }
        public Task GoBackModal()
        {
            return Shell.Current.Navigation.PopModalAsync();
        }
        Task IRouting.GoBack()
        {
            throw new NotImplementedException();
        }
        Task IRouting.GoBackModal()
        {
            throw new NotImplementedException();
        }
        Task IRouting.NavigateTo(string route)
        {
            throw new NotImplementedException();
        }
    }
}
