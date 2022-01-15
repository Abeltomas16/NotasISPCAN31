using NotasISPCAN.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotasISPCAN.Features.Service
{
    public interface IDocente
    {
        Task<string> Cadastrar(UsuarioDTO entidade, List<CadeiraDTO> cadeiras);
        Task<string> Alterar(UsuarioDTO entidade, List<CadeiraDTO> cadeiras);
        Task<string> Apagar(UsuarioDTO usuarioDTO);
        Task<List<UsuarioDTO>> ListarTodos();
        Task<List<NotasDTO>> MostrarNotas(string keyCadeira);
        Task<List<NotasCadeiras>> MostrarNotasRelatorio(string keyCadeira);
        Task<List<NotasDTO>> MostrarNotas();
        Task<UsuarioDTO> Pesquisar(string token);
    }
}
