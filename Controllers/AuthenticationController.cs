using BasicCrudAPI.Classes;
using BasicCrudAPI.Controllers.Base;
using BasicCrudAPI.Models.AdminModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        public UserHelper _userHelper { get; set; }
        public AuthenticationController(IConfiguration config) : base(config)
        {
            _userHelper = new UserHelper(config, _dbHelper);
        }
        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(LoginUser login)
        {
            if (!_userHelper.LoginUser(login.Username, login.Password, out var user))
            {
                return Unauthorized("Failed to login to the account");
            }

            return Ok(_userHelper.GetUserToken(user));
        }
        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            return Ok(_userHelper.RefreshUserToken(refreshToken));
        }

        [HttpPost("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(SignUpUser user)
        {
            if (_userHelper.UsernameExists(user.Username))
            {
                return Conflict("User already exists with the specified username");
            }

            if (_userHelper.EmailExists(user.Email))
            {
                return Conflict("User already exists with the specified email");
            }

            _userHelper.RegisterUser(user.Username, user.Email, user.Password);
            return Ok();
        }

      
    }
}
