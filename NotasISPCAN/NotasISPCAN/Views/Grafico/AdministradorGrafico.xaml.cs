using Microcharts;
using NotasISPCAN.Models;
using NotasISPCAN.ViewModel;
using SkiaSharp;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.Views.Grafico
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdministradorGrafico : ContentPage
    {
        DocenteViewModel DocentesViewModel { get; } = Locator.Current.GetService<DocenteViewModel>();
        public AdministradorGrafico()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            List<ChartEntry> Entryparcelar1 = new List<ChartEntry>();
            List<ChartEntry> Entryparcelar2 = new List<ChartEntry>();
            List<ChartEntry> EntryDesempenho = new List<ChartEntry>();
            List<ChartEntry> EntrySemestre = new List<ChartEntry>();
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var load = await MaterialDialog.Instance.LoadingDialogAsync(message: "Caregando");
                    List<NotasDTO> notas = await DocentesViewModel.mostrarNotas();
                    await load.DismissAsync();

                    if (notas.Count <= 0)
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: "Sem notas", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                            new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                            {
                                BackgroundColor = Color.Orange,
                                MessageTextColor = Color.Black
                            });
                        return;
                    }

                    string keyMelhorAluno = string.Empty;
                    int _melhorAluno = 0;
                    notas.ForEach((n) =>
                    {
                        int _nota_1 = int.Parse(n.Nota1);
                        int _nota_2 = int.Parse(n.Nota2);
                        int soma = _nota_1 + _nota_2;
                        if (soma > _melhorAluno)
                        {
                            _melhorAluno = soma;
                            keyMelhorAluno = n.KeyAluno;
                        }
                    });
                    UsuarioDTO usuarioMelhorAluno = await DocentesViewModel.MelhorAluno(keyMelhorAluno);
                    if (usuarioMelhorAluno.Name == null)
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: "Aluno não encontrado", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                             new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                             {
                                 BackgroundColor = Color.Orange,
                                 MessageTextColor = Color.Black
                             });
                        return;
                    }
                    melhorAluno.Text = usuarioMelhorAluno.Name;
                    #region PRIMEIRA PARCELAR
                    int positiva1 = notas.Count(x => int.Parse(x.Nota1) >= 10);
                    int negativa1 = notas.Count(x => int.Parse(x.Nota1) < 10);
                    Entryparcelar1.Add(new ChartEntry(positiva1)
                    {
                        ValueLabel = positiva1.ToString(),
                        Label = "APTO",
                        Color = SKColor.Parse("#46D57B")
                    }); ;
                    Entryparcelar1.Add(new ChartEntry(negativa1)
                    {
                        ValueLabel = negativa1.ToString(),
                        Label = "N/A",
                        Color = SKColors.Red
                    });
                    parcelar1.Chart = new DonutChart() { Entries = Entryparcelar1, LabelTextSize = 25 };
                    #endregion
                    #region PARCELAR 2
                    int positiva2 = notas.Count(x => int.Parse(x.Nota2) >= 10);
                    int negativa2 = notas.Count(x => int.Parse(x.Nota2) < 10);
                    Entryparcelar2.Add(new ChartEntry(positiva2)
                    {
                        ValueLabel = positiva2.ToString(),
                        Label = "APTO",
                        Color = SKColor.Parse("#46D57B")
                    }); ;
                    Entryparcelar2.Add(new ChartEntry(negativa2)
                    {
                        ValueLabel = negativa2.ToString(),
                        Label = "N/A",
                        Color = SKColors.Red
                    });
                    parcelar2.Chart = new DonutChart() { Entries = Entryparcelar2, LabelTextSize = 25 };
                    #endregion
                    #region DESEMPENHO
                    int index = notas.Count();
                    int desempenho1 = notas.Sum(x => int.Parse(x.Nota1));
                    int desempenho2 = notas.Sum(x => int.Parse(x.Nota2));

                    float desempenho_1 = desempenho1 / index;
                    float desempenho_2 = desempenho2 / index;

                    EntryDesempenho.Add(new ChartEntry(desempenho_1)
                    {
                        ValueLabel = desempenho_1.ToString(),
                        Label = "1ª parcelar",
                        Color = SKColors.Red
                    });
                    EntryDesempenho.Add(new ChartEntry(desempenho_2)
                    {
                        ValueLabel = desempenho_2.ToString(),
                        Label = "2ª parcelar",
                        Color = SKColor.Parse("#46D57B")
                    });
                    desempenho.Chart = new LineChart() { Entries = EntryDesempenho, LabelTextSize = 25 };
                    #endregion
                    #region SEMESTRE
                    int positivasSemestre = notas.Count(x => (int.Parse(x.Nota1) + int.Parse(x.Nota2)) / 2 >= 10);
                    int negativasSemestre = notas.Count(x => (int.Parse(x.Nota1) + int.Parse(x.Nota2)) / 2 < 10);
                    EntrySemestre.Add(new ChartEntry(positivasSemestre)
                    {
                        ValueLabel = positivasSemestre.ToString(),
                        Label = "APTO",
                        Color = SKColor.Parse("#00FEFE"),
                        TextColor = SKColors.Black
                    });
                    EntrySemestre.Add(new ChartEntry(negativasSemestre)
                    {
                        ValueLabel = negativasSemestre.ToString(),
                        Label = "N/A",
                        Color = SKColors.Red,
                        TextColor = SKColors.Black
                    });
                    semestre.Chart = new BarChart()
                    {
                        Entries = EntrySemestre,
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