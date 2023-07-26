using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationSystem.Repositories;
using UserAuthenticationSystem.Types;
using UserAuthenticationSystem.ViewModels;

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
        [HttpPost("/Register")]
        public async Task<IActionResult> Register(UserAccountViewModel uavm, [FromQuery]UserLoginDataViewModel uldvm)
        {
            int savingDone = await _userRepo.Register(uldvm, uavm);
            if (savingDone != -1)
            {
                return RedirectToAction("GetNames");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, savingDone);
            }
        }
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetNames()
        {
            var usernames = await _userRepo.GetUserNames();
            return !HttpContext.User.Identity.IsAuthenticated ? Unauthorized() : Ok(usernames);
        }
    }
}
