using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using net_core_demo_api.Interface;
using net_core_demo_api.Model;

namespace net_core_demo_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserManagementController : ControllerBase
    {
        private readonly ILogger<UserManagementController> _logger;
        private readonly IUserManagementService _userManagementService;
        public UserManagementController(ILogger<UserManagementController> logger, IUserManagementService userManagementService)
        {
            _logger = logger;
            _userManagementService = userManagementService;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _userManagementService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost(Name = "GetUsersById")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersById(string id)
        {
            var users = await _userManagementService.GetUsersByIdAsync(id);
            return Ok(users);
        }

        //[HttpPost(Name = "Login")]
        //public async Task<ActionResult<IEnumerable<User>>> Login(string id,string password)
        //{
        //    var users = await _userManagementService.LoginAsync(id,password);
        //    return Ok(users);
        //}
    }
}