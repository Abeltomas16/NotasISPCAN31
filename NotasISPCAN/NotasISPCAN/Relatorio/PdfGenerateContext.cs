using NotasISPCAN.Relatorio.Interfaces;
using System.Data;
using System.Threading.Tasks;

namespace NotasISPCAN.Relatorio
{
    public class PdfGenerateContext<LS> where LS : Igenerate, new()
    {
        Igenerate Contexto=new LS();
        public Task CriarPdf(DataTable fonteDados)
        {
            return Contexto.CriarPdf(fonteDados);
        }
    }
}
