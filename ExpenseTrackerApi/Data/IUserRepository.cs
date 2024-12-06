using ExpenseTracker.Models;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
    }
}
