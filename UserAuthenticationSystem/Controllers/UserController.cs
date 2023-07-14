using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationSystem.Repositories;
using UserAuthenticationSystem.Types;

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
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetNames()
        {
            var usernames = await _userRepo.GetUserNames();
            return !HttpContext.User.Identity.IsAuthenticated ? Unauthorized() : Ok(usernames);
        }
    }
}
