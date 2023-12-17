using BasicCrudAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BasicCrudAPI.Annotations;
using BasicCrudAPI.Controllers.Base;
using BasicCrudAPI.Models.AdminModels;

namespace BasicCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Admin]
    public class UserController :BaseController
    {
        public UserController(IConfiguration config) : base(config)
        {
        }
        [HttpPost("ApproveUser")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var user = _dbHelper.SelectRecord<User>(id);
            user.IsApproved = true;
            _dbHelper.UpsertRecord(user);
            return Ok();
        }
        [HttpPost("DeactivateUser")]
        public async Task<IActionResult> DeactivateUser(string id)
        {
            var user = _dbHelper.SelectRecord<User>(id);
            user.IsApproved = false;
            _dbHelper.UpsertRecord(user);
            return Ok();
        }

        [HttpPost("PromoteUserToAdmin")]
        public async Task<IActionResult> PromoteUserToAdmin(string id)
        {
            var user = _dbHelper.SelectRecord<User>(id);
            user.IsAdmin = true;
            _dbHelper.UpsertRecord(user);
            return Ok();
        }
        [HttpPost("DemoteUserFromAdmin")]
        public async Task<IActionResult>DemoteUserToAdmin(string id)
        {
            var user = _dbHelper.SelectRecord<User>(id);
            user.IsAdmin = false;
            _dbHelper.UpsertRecord(user);
            return Ok();
        }

    }
}
