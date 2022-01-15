using System.Data;
using System.Threading.Tasks;

namespace NotasISPCAN.Relatorio.Interfaces
{
    public interface Igenerate
    {
        Task CriarPdf(DataTable fonteDados);
    }
}
