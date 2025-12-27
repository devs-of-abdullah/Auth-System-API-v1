
namespace Business
{
    public interface IUserService
    {
        Task<bool> Register(string email, string password);
        Task<string> Login(string email, string password);
    }
}
