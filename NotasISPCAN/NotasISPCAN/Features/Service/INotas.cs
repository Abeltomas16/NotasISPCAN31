using NotasISPCAN.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotasISPCAN.Features.Service
{
    public interface INotas
    {
        Task<NotasDTO> listarPorCadeira(string keycadeira, string keyEstudante);
        Task<List<NotasDTO>> listarPorCadeira(string keycadeira);
        Task<List<NotasDTO>> listarPorCadeira();
        Task<List<NotasCadeirasDocente>> listarPorAluno(string keyEstudante);
        Task<string> Cadastrar(NotasDTO actividade);
        Task<string> Alterar(NotasDTO entidade);
    }
}
