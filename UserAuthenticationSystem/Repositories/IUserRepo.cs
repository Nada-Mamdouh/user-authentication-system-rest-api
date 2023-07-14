namespace UserAuthenticationSystem.Repositories
{
    public interface IUserRepo
    {
        Task<List<string>> GetUserNames();
    }
}
