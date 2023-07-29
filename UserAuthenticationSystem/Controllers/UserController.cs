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
            /*** Today's work 29/7: implemented login function partially;
             * could use jwt to actually generate token for user when trying to login 
             */
            /** TODO: For next session
             * 1- Check if the user couldn't login then redirect to Register action: 
             * 2- While generating jwt token, I need to verify password hash which means I need to hash incoming password and compare 
             * it to the hash already exists in db, there's already verifypassord function inside "HashPasswordService" that do just that.
             * 3- Complete login feature.
             * 4- I need to work on that when user register for first time send confirmation token to his mail, to check his mail validity.
             */
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
