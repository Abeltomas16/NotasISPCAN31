using NotasISPCAN.Models;
using Splat;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotasISPCAN.Features.Service
{
    public class DocenteService : IDocente
    {
        IUsuario dbLogin { get; } = Locator.Current.GetService<IUsuario>();
        ICadeira dbCadeira { get; } = Locator.Current.GetService<ICadeira>();
        INotas dbNotas { get; } = Locator.Current.GetService<INotas>();
        public async Task<string> Cadastrar(UsuarioDTO entidade, List<CadeiraDTO> cadeiras)
        {
            List<Task> tarefas = new List<Task>();
            string retorno = await dbLogin.Cadastrar(entidade);
            foreach (CadeiraDTO item in cadeiras)
                tarefas.Add(dbCadeira.DocenteCadeira(item, retorno));
            await Task.WhenAll(tarefas);
            return "Docente cadastrado com sucesso";
        }
        public async Task<string> Alterar(UsuarioDTO entidade, List<CadeiraDTO> cadeiras)
        {
            await dbLogin.Alterar(entidade, entidade.Key);
            await dbCadeira.apagarCadeiraProf(entidade.Token);
            List<Task> tarefas = new List<Task>();

            foreach (CadeiraDTO item in cadeiras)
                tarefas.Add(dbCadeira.DocenteCadeira(item, entidade.Token));
            await Task.WhenAll(tarefas);
            return "Docente alterado com sucesso";
        }
        public async Task<List<UsuarioDTO>> ListarTodos()
        {
            var dados = (await dbLogin.ListarTodos()).Where(y => y.Categoria == "Professor");
            return dados.ToList();
        }
        public async Task<string> Apagar(UsuarioDTO usuarioDTO)
        {
            string retorno = await dbLogin.Apagar(usuarioDTO);
            return retorno;
        }

        public async Task<UsuarioDTO> Pesquisar(string token)
        {
            var dados = await dbLogin.Pesquisar(token);
            return dados;
        }

        public async Task<List<NotasDTO>> MostrarNotas(string keyCadeira)
        {
            var dados = await dbNotas.listarPorCadeira(keyCadeira);
            return dados;
        }
        public async Task<List<NotasDTO>> MostrarNotas()
        {
            var dados = await dbNotas.listarPorCadeira();
            return dados;
        }
    }
}

