namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface ILoginService
    {
        bool VerifyPassword(string typedPassword, string storedPassword);
    }
}
