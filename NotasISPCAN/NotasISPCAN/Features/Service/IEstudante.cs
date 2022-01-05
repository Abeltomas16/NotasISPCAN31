using NotasISPCAN.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotasISPCAN.Features.Service
{
    public interface IEstudante
    {
        Task<List<NotasCadeirasDocente>> ListarNotas(string Keyaluno);
    }
}
