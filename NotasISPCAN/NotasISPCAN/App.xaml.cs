using NotasISPCAN.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotasISPCAN
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Page1());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
