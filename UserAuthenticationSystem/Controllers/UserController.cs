using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationSystem.Repositories;

namespace UserAuthenticationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserRepo _userRepo;
        public UserController(IUserRepo userRepo)
        {
            this._userRepo = userRepo;
        }
        [HttpGet]
        public async Task<List<string>> GetNames()
        {
            return await _userRepo.GetUserNames();
        } 
    }
}
