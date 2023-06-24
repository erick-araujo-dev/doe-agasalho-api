using DoeAgasalhoApiV2._0.Services.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class LoginService : ILoginService
    {
        public bool VerifyPassword(string typedPassword, string storedPassword)
        {
            return typedPassword == storedPassword;
        }
    }
}
