using NotasISPCAN.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotasISPCAN.Features.Service
{
    public interface IActividades
    {
        Task<List<ActividadeDTO>> listarTodos();
        Task<string> Cadastrar(ActividadeDTO actividade);
        Task<string> Alterar(ActividadeDTO entidade, string key);
        Task<string> Apagar(string key);

    }
}
