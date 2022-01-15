using Firebase.Database;
using Firebase.Database.Query;
using NotasISPCAN.Models;
using Splat;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotasISPCAN.Features.Service
{
    public class NotasService : INotas
    {
        FirebaseClient dbCliente { get; } = Locator.Current.GetService<FirebaseClient>();
        ICadeira cadeiraService;
        IUsuario dbLogin { get; } = Locator.Current.GetService<IUsuario>();
        public NotasService(FirebaseClient client = null, ICadeira cadeira = null, IUsuario docente = null)
        {
            dbCliente = client ?? Locator.Current.GetService<FirebaseClient>();
            cadeiraService = cadeira ?? Locator.Current.GetService<ICadeira>();
            dbLogin = docente ?? Locator.Current.GetService<IUsuario>();
        }
        public async Task<string> Alterar(NotasDTO notas)
        {
            string key = notas.Key;
            notas.Key = null;
            await dbCliente.Child("notas")
                            .Child(key)
                            .PutAsync(notas);
            return "Nota Alterada";
        }

        public async Task<string> Cadastrar(NotasDTO notas)
        {
            await dbCliente.Child("notas")
                 .PostAsync(notas);
            return "Nota cadastrada";
        }

        public async Task<NotasDTO> listarPorCadeira(string keycadeira, string keyEstudante)
        {
            var Nota = (await dbCliente.Child("notas")
                            .OnceAsync<NotasDTO>())
                            .Select(z => new NotasDTO
                            {
                                Key = z.Key,
                                KeyAluno = z.Object.KeyAluno,
                                KeyCadeira = z.Object.KeyCadeira,
                                Nota1 = z.Object.Nota1 ?? "0",
                                Nota2 = z.Object.Nota2 ?? "0"

                            })
                            .Where(x => x.KeyAluno == keyEstudante && x.KeyCadeira == keycadeira)
                            .FirstOrDefault();
            return Nota;
        }
        public async Task<List<NotasCadeirasDocente>> listarPorAluno(string keyEstudante)
        {
            List<CadeiraDTO> cadeiras = await cadeiraService.listarTodos();
            var docent = (await dbLogin.ListarTodos()).Where(y => y.Categoria == "Professor");

            List<NotasDTO> Notas = (await dbCliente.Child("notas")
                            .OnceAsync<NotasDTO>())
                            .Select(z => new NotasDTO
                            {
                                Key = z.Key,
                                KeyAluno = z.Object.KeyAluno,
                                KeyCadeira = z.Object.KeyCadeira,
                                Nota1 = z.Object.Nota1,
                                Nota2 = z.Object.Nota2
                            })
                            .Where(x => x.KeyAluno == keyEstudante).ToList();

            var notaJoincadeira = from i in Notas
                                  join d in cadeiras
                                  on i.KeyCadeira equals d.IDCadeira
                                  select new
                                  {
                                      i.Nota1,
                                      Nota2 = i.Nota2 ?? "0",
                                      NomeCadeira = d.Name,
                                      d.Docente
                                  };
            var cadeiraJoinDocente = from i in notaJoincadeira
                                     join d in docent
                                     on i.Docente equals d.Token
                                     select new
                                     {
                                         i.Nota1,
                                         i.Nota2,
                                         i.NomeCadeira,
                                         NomeDocente = d.Name
                                     };
            List<NotasCadeirasDocente> lista = new List<NotasCadeirasDocente>();
            foreach (var item in cadeiraJoinDocente)
            {
                string cadeira_ = item.NomeCadeira;
                string prof_ = item.NomeDocente;
                if (item.NomeCadeira.Length > 11)
                    cadeira_ = item.NomeCadeira.Substring(0, 11);

                if (item.NomeDocente.Length > 11)
                    prof_ = item.NomeDocente.Substring(0, 10);

                lista.Add(new NotasCadeirasDocente
                {
                    NomeCadeira = cadeira_,
                    NomeDocente = prof_,
                    Nota1 = item.Nota1,
                    Nota2 = item.Nota2
                });
            }
            return lista;
        }

        public async Task<List<NotasDTO>> listarPorCadeira(string keycadeira)
        {
            var notas = (await dbCliente.Child("notas")
                        .OnceAsync<NotasDTO>()
                        ).Select(n => new NotasDTO
                        {
                            KeyCadeira = n.Object.KeyCadeira,
                            Nota1 = n.Object.Nota1 ?? "0",
                            Nota2 = n.Object.Nota2 ?? "0"
                        }).Where(x => x.KeyCadeira == keycadeira);
            return notas.ToList();
        }
        public async Task<List<NotasCadeiras>> listarPorCadeiraRelatorio(string keycadeira)
        {
            List<NotasCadeiras> notasCadeiras = new List<NotasCadeiras>();
            var estudantes = (await dbLogin.ListarTodos()).Where(y => y.Categoria == "Estudante");

            var notas = (await dbCliente.Child("notas")
                        .OnceAsync<NotasDTO>()
                        ).Select(n => new NotasDTO
                        {
                            KeyCadeira = n.Object.KeyCadeira,
                            KeyAluno = n.Object.KeyAluno,
                            Nota1 = n.Object.Nota1 ?? "0",
                            Nota2 = n.Object.Nota2 ?? "0"
                        }).Where(x => x.KeyCadeira == keycadeira);

            var uniao = from n in notas
                        join a in estudantes
                        on n.KeyAluno equals a.Key
                        select new
                        {
                            a.Name,
                            n.Nota1,
                            n.Nota2
                        };
            foreach (var item in uniao)
            {
                double n1 = double.Parse(item.Nota1);
                double n2 = double.Parse(item.Nota2);
                double media = (n1 + n2) / 2;
                notasCadeiras.Add(new NotasCadeiras
                {
                    NomeAluno = item.Name,
                    Nota1 = item.Nota1,
                    Nota2 = item.Nota2,
                    Media = media
                });
            }
            return notasCadeiras;
        }
        public async Task<List<NotasDTO>> listarPorCadeira()
        {
            var notas = (await dbCliente.Child("notas")
                        .OnceAsync<NotasDTO>()
                        ).Select(n => new NotasDTO
                        {
                            KeyCadeira = n.Object.KeyCadeira,
                            KeyAluno = n.Object.KeyAluno,
                            Nota1 = n.Object.Nota1 ?? "0",
                            Nota2 = n.Object.Nota2 ?? "0"
                        });
            return notas.ToList();
        }
    }
}
