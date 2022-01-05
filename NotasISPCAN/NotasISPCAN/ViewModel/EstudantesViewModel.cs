using NotasISPCAN.Features.Service;
using NotasISPCAN.Models;
using Splat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotasISPCAN.ViewModel
{
    public class EstudantesViewModel
    {
        IEstudante estudanteService;
        public EstudantesViewModel(IEstudante estudantes = null)
        {
            estudanteService = estudantes ?? Locator.Current.GetService<IEstudante>();
        }
        public async Task<List<NotasCadeirasDocente>> listar(string keyEstudante)
        {
            var notas = await estudanteService.ListarNotas(keyEstudante);
            return notas;
        }
    }
}
