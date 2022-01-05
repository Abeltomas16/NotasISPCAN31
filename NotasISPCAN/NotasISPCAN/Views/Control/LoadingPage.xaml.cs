using NotasISPCAN.ViewModel;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        LoadingViewModel ViewModel { get; set; } = Locator.Current.GetService<LoadingViewModel>();
        public LoadingPage()
        {
            InitializeComponent();
            ViewModel.Init();
        }
    }
}
