using MongoDB.Driver;
using net_core_demo_api.Interface;
using net_core_demo_api.Model;

namespace net_core_demo_api.Service
{
    public class UserManagementService : IUserManagementService
    {

        private readonly ILogger<UserManagementService> _logger;

        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _collection;
        public UserManagementService(IMongoDatabase database, ILogger<UserManagementService> logger)
        {
            _logger = logger;
            _database = database;
            _collection = _database.GetCollection<User>("users_v1");
        }
        public async Task<List<User>> GetUsersAsync() => await _collection.Find(_ => true).ToListAsync();

        public async Task<User?> GetUsersByIdAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateUserAsync(User newUser) =>
            await _collection.InsertOneAsync(newUser);

        public async Task UpdateUserAsync(string id, User updatedUser) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveUserAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        //public async Task LoginAsync(string id, string password)
        //{
        //    var loginResponse = "";
        //    return "tets";
        //}
    }
}
