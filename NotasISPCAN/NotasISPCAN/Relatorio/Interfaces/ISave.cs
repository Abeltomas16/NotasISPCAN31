using System;
using System.IO;
using System.Threading.Tasks;

namespace NotasISPCAN.Relatorio.Interfaces
{
    public interface ISave
    {
        Task SaveAndView(string fileName, String contentType, MemoryStream stream);
    }
}
