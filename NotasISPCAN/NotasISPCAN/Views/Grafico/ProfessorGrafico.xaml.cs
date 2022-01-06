using Microcharts;
using NotasISPCAN.Models;
using NotasISPCAN.ViewModel;
using SkiaSharp;
using Splat;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.Views.Grafico
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfessorGrafico : ContentPage
    {
        DocenteViewModel DocentesViewModel { get; } = Locator.Current.GetService<DocenteViewModel>();
        public ProfessorGrafico()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            List<ChartEntry> entries1 = new List<ChartEntry>();
            List<ChartEntry> entries2 = new List<ChartEntry>();
            List<ChartEntry> entries3 = new List<ChartEntry>();
            IMaterialModalPage load = null;
            try
            {
                string keycadeira = Application.Current.Properties["IDCadeira"].ToString();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    load = await MaterialDialog.Instance.LoadingDialogAsync(message: "Caregando");
                    List<NotasDTO> notas = await DocentesViewModel.mostrarNotas(keycadeira);
                    await load.DismissAsync();
                    #region PRIMEIRA PARCELAR
                    int positivas = 0;
                    int negativas = 0;
                    foreach (NotasDTO item in notas)
                    {
                        int _nota1 = int.TryParse(item.Nota1, out _nota1) ? _nota1 : 0;
                        if (_nota1 >= 10)
                            positivas++;
                        else
                            negativas++;
                    }
                    TotPositivas1.Text = positivas.ToString();
                    TotNegativas1.Text = negativas.ToString();
                    entries1.Add(new ChartEntry(positivas)
                    {
                        ValueLabel = positivas.ToString(),
                        Label = "Aprovados",
                        Color = SKColor.Parse("#00FEFE"),
                        TextColor = SKColors.Black
                    });
                    entries1.Add(new ChartEntry(negativas)
                    {
                        ValueLabel = negativas.ToString(),
                        Label = "Reprovados",
                        Color = SKColors.Red,
                        TextColor = SKColors.Black
                    });
                    grafico1.Chart = new DonutChart()
                    {
                        Entries = entries1,
                        BackgroundColor = SKColors.Transparent,
                        LabelTextSize = 25,
                        AnimationDuration = TimeSpan.FromMilliseconds(2000),
                        IsAnimated = true
                    };
                    #endregion
                    #region SEGUNDA PARCELAR
                    positivas = 0;
                    negativas = 0;
                    foreach (NotasDTO item in notas)
                    {
                        int _nota2 = int.TryParse(item.Nota2, out _nota2) ? _nota2 : 0;
                        if (_nota2 >= 10)
                            positivas++;
                        else
                            negativas++;
                    }
                    TotPositivas2.Text = positivas.ToString();
                    TotNegativas2.Text = negativas.ToString();
                    entries2.Add(new ChartEntry(positivas)
                    {
                        ValueLabel = positivas.ToString(),
                        Label = "Aprovados",
                        Color = SKColor.Parse("#00FEFE"),
                        TextColor = SKColors.Black
                    });
                    entries2.Add(new ChartEntry(negativas)
                    {
                        ValueLabel = negativas.ToString(),
                        Label = "Reprovados",
                        Color = SKColors.Red,
                        TextColor = SKColors.Black
                    });
                    grafico2.Chart = new DonutChart()
                    {
                        Entries = entries2,
                        BackgroundColor = SKColors.Transparent,
                        LabelTextSize = 25,
                        AnimationDuration = TimeSpan.FromMilliseconds(2000),
                        IsAnimated = true
                    };
                    #endregion
                    #region SEMESTRE
                    positivas = 0;
                    negativas = 0;
                    foreach (NotasDTO item in notas)
                    {
                        int _nota1 = int.TryParse(item.Nota1, out _nota1) ? _nota1 : 0;
                        int _nota2 = int.TryParse(item.Nota2, out _nota2) ? _nota2 : 0;
                        double soma = _nota1 + _nota2;
                        if ((soma / 2) >= 10)
                            positivas++;
                        else
                            negativas++;
                    }
                    entries3.Add(new ChartEntry(positivas)
                    {
                        ValueLabel = positivas.ToString(),
                        Label = "Aprovados",
                        Color = SKColor.Parse("#00FEFE"),
                        TextColor = SKColors.Black
                    });
                    entries3.Add(new ChartEntry(negativas)
                    {
                        ValueLabel = negativas.ToString(),
                        Label = "Reprovados",
                        Color = SKColors.Red,
                        TextColor = SKColors.Black
                    });
                    grafico3.Chart = new BarChart()
                    {
                        Entries = entries3,
                        BackgroundColor = SKColors.Transparent,
                        LabelTextSize = 25,
                        AnimationDuration = TimeSpan.FromMilliseconds(2000),
                        IsAnimated = true
                    };
                    #endregion
                });
            }
            catch (Exception)
            {
                if (!(load is null))
                    await load.DismissAsync();

                await MaterialDialog.Instance.SnackbarAsync(message: "Erro contacte o administrador", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                   new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                   {
                       BackgroundColor = Color.Orange,
                       MessageTextColor = Color.Black
                   });
            }
        }
    }
}