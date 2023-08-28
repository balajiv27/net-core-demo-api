using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using net_core_demo_api.Interface;
using net_core_demo_api.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace net_core_demo_api.Service
{
    public class UserManagementService : IUserManagementService
    {

        private readonly ILogger<UserManagementService> _logger;

        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _collection;
        private readonly IConfiguration _configuration;
        public UserManagementService(IMongoDatabase database, ILogger<UserManagementService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _database = database;
            _configuration = configuration;
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

        public async Task<string> LoginAsync(string id, string password)
        { 
            var validUserResponse = await IsValidUser(id, password, _collection);
            if (validUserResponse != AppConstants.INVALID_USER)
            {
                var token = GenerateJwtToken(validUserResponse);
                return token;
            }
            return AppConstants.NOT_AUTHORIZED;
        }

        private async Task<string> IsValidUser(string id, string password, IMongoCollection<User> userCollection)
        {
            var filter = Builders<User>.Filter;
            var query =filter.Eq(x => x.Email, id); // filter.Eq(x => x.Id, id) | 

            var user = await userCollection.Find(query).FirstOrDefaultAsync();

            if (user != null)
            {
                return user.Email;
            }

            return AppConstants.INVALID_USER;
        }




        private string GenerateJwtToken(string email)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
