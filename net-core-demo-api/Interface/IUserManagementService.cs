using net_core_demo_api.Model;

namespace net_core_demo_api.Interface
{
    public interface IUserManagementService
    {
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUsersByIdAsync(string id);
        Task CreateUserAsync(User newUser);
        Task UpdateUserAsync(string id, User updatedUser);
        Task RemoveUserAsync(string id);
        Task<string> LoginAsync(string id, string password);


    }
}
