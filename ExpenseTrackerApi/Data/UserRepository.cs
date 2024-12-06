using ExpenseTracker.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        public async Task<User> GetUserByIdAsync(string id) =>
            await _users.Find(user => user.Id.ToString() == id).FirstOrDefaultAsync();

        public async Task<User> GetUserByUsernameAsync(string username) =>
            await _users.Find(user => user.Username == username).FirstOrDefaultAsync();

        public async Task CreateUserAsync(User user) =>
            await _users.InsertOneAsync(user);
    }
}
