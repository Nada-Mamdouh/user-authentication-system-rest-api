using UserAuthenticationSystem.Models;
namespace UserAuthenticationSystem.Repositories
{
    public class UserRepo : IUserRepo
    {
        UserAuthenticationSystemDbContext _dbContext;
        public UserRepo(UserAuthenticationSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<string>> GetUserNames()
        {
            List<string> userNames = new List<string>();
            foreach(var user in _dbContext.UserAccounts)
            {
                userNames.Add(user.FirstName);
            }
            return await Task.FromResult<List<string>>(userNames);
        }
    }
}
