using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _userManagementService.GetUsersAsync();
            return Ok(users);
        }

        //[HttpPost(Name = "GetUsersById/{id}")]
        //public async Task<ActionResult<IEnumerable<User>>> GetUsersById(string id)
        //{
        //    var users = await _userManagementService.GetUsersByIdAsync(id);
        //    return Ok(users);
        //}

        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login(string id, string password)
        {
            ApiResponse<string> apiResponse = new ApiResponse<string>() { };

            var token = await _userManagementService.LoginAsync(id, password);
            if (!string.IsNullOrEmpty(token))
            {
                apiResponse.Message = "Token Acquired";
                apiResponse.Success = true;
                apiResponse.Data = token;
            }
            return Ok(apiResponse);
          
        }
    }
}