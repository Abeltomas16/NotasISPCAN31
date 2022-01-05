using System;
using System.Globalization;
using Xamarin.Forms;

namespace NotasISPCAN.Views
{
    internal class TypeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color cor = Color.FromRgb(235, 181, 7);
            Random random = new Random();
            int indice = random.Next(1, 9);

            switch (indice)
            {
                case 1:
                    cor = Color.FromRgb(235, 181, 7);
                    break;
                case 2:
                    cor = Color.FromRgb(64, 64, 150);
                    break;
                case 3:
                    cor = Color.FromRgb(70, 213, 123);
                    break;
                case 4:
                    cor = Color.FromRgb(0, 119, 247);
                    break;
                case 5:
                    cor = Color.FromRgb(0, 49, 102);
                    break;
                case 6:
                    cor = Color.FromRgb(213, 51, 67);
                    break;
                case 7:
                    cor = Color.FromRgb(214, 38, 136);
                    break;
                case 8:
                    cor = Color.FromRgb(204, 102, 51);
                    break;
                case 9:
                    cor = Color.FromRgb(0, 153, 255);
                    break;
            }
            return cor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
