using ExpenseTracker.Data;
using ExpenseTracker.Models;
using System.Threading.Tasks;


namespace ExpenseTracker.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(string id) =>
            await _userRepository.GetUserByIdAsync(id);

        public async Task<User> GetUserByUsernameAsync(string username) =>
            await _userRepository.GetUserByUsernameAsync(username);

        public async Task CreateUserAsync(User user){
            await _userRepository.CreateUserAsync(user);
        }

         public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
