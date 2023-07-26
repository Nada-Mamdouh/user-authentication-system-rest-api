using UserAuthenticationSystem.Models;
using UserAuthenticationSystem.Services;
using UserAuthenticationSystem.ViewModels;

namespace UserAuthenticationSystem.Repositories
{
    public class UserRepo : IUserRepo
    {
        UserAuthenticationSystemDbContext _dbContext;
        IHashAlgoRepo _hashAlgoRepo;
        public UserRepo(UserAuthenticationSystemDbContext dbContext, IHashAlgoRepo hashAlgoRepo)
        {
            _dbContext = dbContext;
            _hashAlgoRepo = hashAlgoRepo;   
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
        public async Task<bool> UserExists(string email_address)
        {
            var user = await this.GetUserByEmail(email_address);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public async Task<UserLoginDatum> GetUserByEmail(string emailaddress)
        {
            var user = _dbContext.UserLoginData.FirstOrDefault(u => u.EmailAddress == emailaddress);
            return user;
        }

        public async Task<int> Register(UserLoginDataViewModel ulvm, UserAccountViewModel uvm)
        {
            UserAccount ua = new UserAccount();
            UserLoginDatum uld = new UserLoginDatum();

            ua.FirstName = uvm.FirstName;
            ua.LastName = uvm.LastName;
            ua.Gender = uvm.Gender;
            ua.DateOfBirth = uvm.DateOfBirth;

            uld.EmailAddress = ulvm.Email;
            uld.LoginName = ulvm.LoginName;
            byte[] generatedSalt;
            var PasswordHash = HashPasswordService.HashPasword(ulvm.Password, out generatedSalt);
            uld.PasswordHash = PasswordHash;
            uld.PasswordSalt = Convert.ToHexString(generatedSalt);
            Console.WriteLine($"Password hash: {PasswordHash}");
            Console.WriteLine($"Generated salt: {Convert.ToHexString(generatedSalt)}");

            string AlgorithmName = HashPasswordService.hashAlgorithm.ToString();
            var hashingalgoid = this._hashAlgoRepo.AddHashAlgo(AlgorithmName);
            uld.HashAlgorithmId = hashingalgoid;

            bool userExists = await UserExists(uld.EmailAddress);
            if (!userExists)
            {
                _dbContext.UserAccounts.Add(ua);
                var accres = _dbContext.SaveChanges();
                if (accres != 0)
                {
                    uld.UserId = ua.UserId;
                    uld.User = ua;
                    _dbContext.UserLoginData.Add(uld);
                    var result = _dbContext.SaveChanges();
                    if (result != 0)
                    {
                        Console.WriteLine("registered successfully!");
                        return ua.UserId;
                    }

                }
            }
            else if (userExists)
            {
                Console.WriteLine("Already existed!");
                return uld.UserId;
            }
            Console.WriteLine("Failed to register and it doesn't already exist!");
            return -1;
        }
    }
}
