using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationSystem.Repositories;
using UserAuthenticationSystem.Services;
using UserAuthenticationSystem.Types;
using UserAuthenticationSystem.ViewModels;

namespace UserAuthenticationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserRepo _userRepo;
        IJWTTokenServices _jwtTokenServices;
        public UserController(IUserRepo userRepo, IJWTTokenServices jwtTokenServices)
        {
            _userRepo = userRepo;
            _jwtTokenServices = jwtTokenServices;
        }
        [HttpPost]
        [Route("Register")]
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
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserLoginDataViewModel users)
        {
            var token = _jwtTokenServices.Authenticate(users);

            if (token == null)
            {
                Console.WriteLine("=======================non-authenticated===================");
                return Ok(new { Message = "Unauthorized" });
            }
            Console.WriteLine("=======================authenticated===================");
            return Ok(token);
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
