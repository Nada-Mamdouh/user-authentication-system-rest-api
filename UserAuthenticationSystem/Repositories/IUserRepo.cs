using UserAuthenticationSystem.Models;
using UserAuthenticationSystem.ViewModels;

namespace UserAuthenticationSystem.Repositories
{
    public interface IUserRepo
    {
         Task<int> Register(UserLoginDataViewModel ulvm, UserAccountViewModel uvm);
        Task<bool> Authenticate(string username, string password);
        Task<List<string>> GetUserNames();
        Task<UserLoginDatum> GetUserByEmail(string emailaddress);
        Task<bool> UserExists(string emailaddress);
        List<UserLoginDatum> GetUsers();

    }
}
