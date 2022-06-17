using System.Threading.Tasks;
using WebApp.Core.Models;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp.Service.Services.Accounts
{
    public interface IUserService
    {
        Task<TokenModel> LoginGenerateJwtTokenAsync(string userName, string password);
        Task<object> AddUserAsync(User user, string password);
        Task<object> AddRoleAsync(Role role);
        Task<object> AssignRoleAsync(string username, string role);
    }
}