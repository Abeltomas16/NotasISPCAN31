using Firebase.Auth;
using Newtonsoft.Json;
using Splat;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NotasISPCAN.Features.Service
{
    public class LoginService : ILogin
    {
        public class v
        {
            public string idToken { get; set; }
        }
        FirebaseAuthProvider auth { get; } = Locator.Current.GetService<FirebaseAuthProvider>();
        public async Task<string> SignIn(string email, string password)
        {
            var content = await auth.SignInWithEmailAndPasswordAsync(email, password);
            var result = await content.GetFreshAuthAsync();
            var serialized = JsonConvert.SerializeObject(result);
            var userId = result.User.LocalId;
            Preferences.Set("MyEmail", email);
            Preferences.Set("MySenha", password);
            Preferences.Set("MyFirebaseToken", serialized);
            return await Task.FromResult(userId);
        }
        public async Task<string> SignUp(string email, string password, string displayname)
        {
            var conn = await auth.CreateUserWithEmailAndPasswordAsync(email, password, displayname);
            var id = conn.User.LocalId;
            return await Task.FromResult(id);
        }
        public async Task DeleteAccount(string email, string password)
        {
            var content = await auth.SignInWithEmailAndPasswordAsync(email, password);
            var result = content.FirebaseToken;
            await auth.DeleteUserAsync(result);
            Preferences.Remove("MyEmail");
            Preferences.Remove("MySenha");
            Preferences.Remove("MyFirebaseToken");
        }
        public void SignOut(string email, string password)
        {
            Preferences.Remove("MyEmail");
            Preferences.Remove("MySenha");
            Preferences.Remove("MyFirebaseToken");

            Application.Current.Properties["NomeUsuario"] = null;
            Application.Current.Properties["TelefoneUsuario"] = null;
            Preferences.Remove("Categoria");
        }

        public async Task<string> UpdateEmail(string newEmail)
        {
            var token = JsonConvert.DeserializeObject<v>(Preferences.Get("MyFirebaseToken", null));
            string a = token.idToken;
            await auth.ChangeUserEmail(a, newEmail);
            Preferences.Remove("MyEmail");
            return "Email alterado";
        }

        public async Task<string> UpdateSenha(string newSenha)
        {
            var email = Preferences.Get("MyEmail", null);
            await auth.SendPasswordResetEmailAsync(email);
            Preferences.Remove("MySenha");
            return "Verifique o seu e-mail para continuar";
        }
    }
}
