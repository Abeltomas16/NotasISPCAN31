using NotasISPCAN.Models;
using NotasISPCAN.ViewModel;
using Splat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocenteNota : ContentPage
    {
        DocenteViewModel DocentesViewModel { get; } = Locator.Current.GetService<DocenteViewModel>();
        public DocenteNota()
        {
            InitializeComponent();
            try
            {
                UsuarioViewModel estudantesViewModel = Locator.Current.GetService<UsuarioViewModel>();
                BindingContext = estudantesViewModel;
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
        private async void ViewEstudante_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UsuarioDTO estudante = (e.CurrentSelection.FirstOrDefault() as UsuarioDTO);

                if (estudante == null) return;

                Locator.CurrentMutable.Register(() => new NotasViewModel(estudante.Key));

                await Shell.Current.Navigation.PushAsync(new NotasDetalhes(estudante));
            }
            catch (Exception)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Erro, contacte o administrador", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                    new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                    {
                        BackgroundColor = Color.Orange,
                        MessageTextColor = Color.Black
                    });
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                string keyCadeira = Application.Current.Properties["IDCadeira"].ToString();
                var notas = await DocentesViewModel.mostrarNotasRelatorio(keyCadeira);
                DataTable dados = converterFromDatatable(notas);
            }
            catch (Exception)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Erro, contacte o administrador", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                    new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                    {
                        BackgroundColor = Color.Orange,
                        MessageTextColor = Color.Black
                    });
            }
        }

        private DataTable converterFromDatatable(List<NotasCadeiras> notas)
        {
            DataTable dados = new DataTable();
            dados.Columns.Add("  Nº", typeof(string));
            dados.Columns.Add("  Nome", typeof(string));
            dados.Columns.Add("  Nota 1º", typeof(string));
            dados.Columns.Add("  Nota 2", typeof(string));
            dados.Columns.Add("  Média", typeof(double));
            int index = 1;
            foreach (var item in notas)
            {
                DataRow linha = dados.NewRow();
                linha[0] = index;
                linha[1] = item.NomeAluno;
                linha[2] = item.Nota1;
                linha[3] = item.Nota2;
                linha[4] = item.Media;
                dados.Rows.Add(linha);
                index++;
            }
            return dados;
        }
    }
}