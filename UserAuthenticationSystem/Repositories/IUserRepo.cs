namespace UserAuthenticationSystem.Repositories
{
    public interface IUserRepo
    {
        Task<bool> Authenticate(string username, string password);
        Task<List<string>> GetUserNames();
    }
}
