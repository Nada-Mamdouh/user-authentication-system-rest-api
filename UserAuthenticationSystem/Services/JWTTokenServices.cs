using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAuthenticationSystem.Repositories;
using UserAuthenticationSystem.Types;
using UserAuthenticationSystem.ViewModels;
using UserAuthenticationSystem.Models;

namespace UserAuthenticationSystem.Services
{
    public class JWTTokenServices : IJWTTokenServices
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepo _userRepository;

        public JWTTokenServices(IConfiguration configuration, IUserRepo userRepo)
        {
            _configuration = configuration;
            _userRepository = userRepo;
        }
        public JWTTokens Authenticate(UserLoginDataViewModel uldvm)
        {
            if (!_userRepository.GetUsers().Any(e => e.LoginName == uldvm.LoginName && e.PasswordHash == uldvm.Password))
            {
                return null;
            }

            var tokenhandler = new JwtSecurityTokenHandler();
            var tkey = Encoding.UTF8.GetBytes(_configuration["JWTToken:this is my custom Secret key for authentication"]);
            var ToeknDescp = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, uldvm.LoginName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tkey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenhandler.CreateToken(ToeknDescp);

            return new JWTTokens { Token = tokenhandler.WriteToken(token) };
        }
    }
}
