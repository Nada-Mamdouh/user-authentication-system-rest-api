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
        public async Task<bool> Authenticate(string username, string password)
        {
            if (await Task.FromResult(_dbContext.UserLoginData.SingleOrDefault(x => x.LoginName == username && x.PasswordHash == password)) != null)
            {
                return true;
            }
            return false;
        }
        public async Task<List<string>> GetUserNames()
        {
            List<string> userNames = new List<string>();
            foreach(var user in _dbContext.UserLoginData)
            {
                userNames?.Add(user?.LoginName);
            }
            return await Task.FromResult(userNames);
        }
    }
}
