using ExpenseTracker.Models;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
