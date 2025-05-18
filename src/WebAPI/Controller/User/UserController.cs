using Business.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.User;

namespace WebAPI.Controller.User
{

    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserRequestModel model)
        {
            try
            {
                var result = await _userService.CreateUserAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error registering user: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var token = await _userService.LoginAsync(username, password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error during login: {ex.Message}");
            }
        }

        [HttpPost("Logout")]
        public  async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("X-Access-Token");
            HttpContext.Response.Cookies.Delete("X-Access-Token-ExpiryInSeconds");
            HttpContext.Response.Cookies.Delete("X-Username");
            HttpContext.Response.Cookies.Delete("X-Refresh-Token");
            HttpContext.Response.Cookies.Delete("X-Refresh-ExpiryInSeconds");

            return Ok("Logged out successfully.");
        }
    }
}
