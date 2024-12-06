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

        public async Task UpdateUserAsync(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update.Set(u => u.Categories, user.Categories);
            await _users.UpdateOneAsync(filter, update);
        }
    }
}
