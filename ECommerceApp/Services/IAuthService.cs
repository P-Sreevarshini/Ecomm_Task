using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface IAuthService
    {
        Task<(int, string)> Registeration(RegistrationModel model, string role);
        Task<(int, string, string, long, string, string)> Login(LoginModel model);
        Task<User> GetUserByIdAsync(long userId);


    }
}