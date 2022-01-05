using NotasISPCAN.Features.Enums;
using NotasISPCAN.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotasISPCAN.Features.Service
{
    public interface ICadeira
    {
        Task<string> inserirOuAlterar(CadeiraDTO cadeira, EnumOperacoes operacoes);
        Task<string> apagar(string key);
        Task removerDocente(CadeiraDTO key);
        Task<string> apagarCadeiraProf(string token);
        Task<string> apagarCadeiraAluno(string token);
        Task<List<CadeiraDTO>> listarTodos();
        Task<List<CadeiraDTO>> CadeiraLivre();
        Task<List<CadeiraDTO>> MostrarPorID(string key);
        Task DocenteCadeira(CadeiraDTO cadeira, string tokenDocente);
    }
}
