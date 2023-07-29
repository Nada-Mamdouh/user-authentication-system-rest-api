using UserAuthenticationSystem.Types;
using UserAuthenticationSystem.ViewModels;

namespace UserAuthenticationSystem.Services
{
    public interface IJWTTokenServices
    {
        JWTTokens Authenticate(UserLoginDataViewModel uldvm);
    }
}
