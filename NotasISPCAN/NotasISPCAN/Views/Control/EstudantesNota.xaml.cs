using NotasISPCAN.ViewModel;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstudantesNota : ContentPage
    {
        EstudantesViewModel _ViewModel { get; } = Locator.Current.GetService<EstudantesViewModel>();
        string[] cores = new string[]
       {
             "#FF1943",
            "#00BFFF",
            "#00CED9",
            "#00CED1",
            "#F58A1F",
            "#282C34",
            "#000000",
        };
        public EstudantesNota()
        {
            InitializeComponent();
            try
            {
                string key = Application.Current.Properties["IDEstudante"].ToString();
                lblNomeAluno.Text = Application.Current.Properties["NomeUsuario"].ToString();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    /*var load = await MaterialDialog.Instance.LoadingDialogAsync(message: "Caregando");
                    List<NotasCadeirasDocente> notas = await _ViewModel.listar(key);
                    load.Dismiss();
                    notass.ItemsSource = notas;
                    List<ChartEntry> entradas1 = new List<ChartEntry>();
                    List<ChartEntry> entradas2 = new List<ChartEntry>();

                    int indice = 0;
                    foreach (NotasCadeirasDocente item in notas)
                    {
                        if (indice == 6) indice = 0;
                        int n1 = int.TryParse(item.Nota1, out n1) ? n1 : 0;
                        int n2 = int.TryParse(item.Nota2, out n2) ? n2 : 0;
                        int soma = n1 + n2;
                        entradas1.Add(new ChartEntry(soma)
                        {
                            Color = SKColor.Parse(cores[indice]),
                            Label = item.NomeCadeira,
                            ValueLabel = soma.ToString()
                        });
                        indice++;
                    }
                    Grafico1.Chart = new LineChart() { Entries = entradas1, BackgroundColor = SKColors.Transparent, LabelTextSize = 25 };
                    indice = 0;
                    foreach (NotasCadeirasDocente item in notas)
                    {
                        if (indice == 6) indice = 0;
                        int n1 = int.TryParse(item.Nota1, out n1) ? n1 : 0;
                        int n2 = int.TryParse(item.Nota2, out n2) ? n2 : 0;
                        float soma = (n1 + n2) / 2;
                        entradas2.Add(new ChartEntry(soma)
                        {
                            Color = SKColor.Parse(cores[indice]),
                            Label = item.NomeCadeira,
                            ValueLabel = soma.ToString()
                        });
                        indice++;
                    }
                    Grafico2.Chart = new DonutChart() { Entries = entradas2, BackgroundColor = SKColors.Transparent, LabelTextSize = 25 };*/

                });
            }
            catch (Exception)
            {
                MaterialDialog.Instance.SnackbarAsync(message: "Erro, contacte o administrador", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                   new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                   {
                       BackgroundColor = Color.Orange,
                       MessageTextColor = Color.Black
                   });
            }
        }
    }
}