using System.Threading.Tasks;

namespace NotasISPCAN.Features.Service
{
    public interface ILogin
    {
        Task<string> SignIn(string email, string password);
        Task<string> SignUp(string email, string password, string displayname);
        Task<string> UpdateEmail(string newEmail);
        Task<string> UpdateSenha(string newSenha);
        Task DeleteAccount(string email, string password);
    }
}
