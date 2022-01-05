using NotasISPCAN.Models;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Sobre : ContentPage
    {
        public Sobre()
        {
            InitializeComponent();
            lblSobre.Text = "Aplicativo com objectivo de maximizar a publicação das notas permitindo aos " +
                                  "estudantes conhecerem os seus resultados a partir de qualquer local.";

            List<UsuarioDTO> usuarios = new List<UsuarioDTO>()
            {
                new UsuarioDTO()
                {
                     Name="ADILSON HEBO"
                },new UsuarioDTO
                {
                    Name="ABILIO JAMBA"
                }, new UsuarioDTO()
                {
                    Name="ANCÉLIO CABINGA"
                }, new UsuarioDTO()
                {
                    Name="ADRIANO JOSÉ"
                },  new UsuarioDTO
                {
                    Name="DANIEL GONÇALVES"
                },new UsuarioDTO
                {
                    Name="DOMINGOS ABREU"
                },new UsuarioDTO
                {
                    Name="EDGAR MARCOS"
                }, new UsuarioDTO
                {
                    Name="EDSON GOLOME"
                },new UsuarioDTO()
                {
                    Name="FERNANDA ANTÓNIO"
                }, new UsuarioDTO()
                {
                    Name="FERNANDO MANUEL"
                },new UsuarioDTO
                {
                    Name="GELSON DOS PRAZERES"
                }, new UsuarioDTO()
                {
                    Name="GRACIETH PASCOAL"
                }, new UsuarioDTO
                {
                    Name="ILÍDIO NHANGA"
                }, new UsuarioDTO
                {
                    Name="INOQUE SEJA"
                },  new UsuarioDTO()
                {
                    Name="JOÃO FERNANDES"
                },     new UsuarioDTO
                {
                    Name="JOÃO FILIPE"
                },new UsuarioDTO()
                {
                    Name="JULIÃO FERREIRA"
                },     new UsuarioDTO
                {
                    Name="LUÍS MARCOS"
                }, new UsuarioDTO()
                {
                    Name="MANUEL DE DEUS"
                },     new UsuarioDTO
                {
                    Name="MARIANO CUXIXIMA"
                }, new UsuarioDTO
                {
                    Name="MARTINS VUNG"
                },     new UsuarioDTO
                {
                    Name="MIGUEL MATAMBA"
                },  new UsuarioDTO
                {
                    Name="NOÉ CAMPOS"
                },
                new UsuarioDTO
                {
                    Name="SALVADOR BRAVO"
                }
            };
            usuarios.Sort(delegate (UsuarioDTO u1, UsuarioDTO p2)
            {
                return u1.Name.CompareTo(p2.Name);
            });
            EstudantesCollectionView.ItemsSource = usuarios;
        }
    }
}