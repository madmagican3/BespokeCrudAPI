using System.Net;
using BasicCrudAPI.Controllers.Base;
using BasicCrudAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasicCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : BaseCrudController<Player>
    {
        public PlayerController(IConfiguration config) : base(config) { }
        [ProducesResponseType(typeof(IEnumerable<Player>), (int)HttpStatusCode.OK)]
        public override async Task<IActionResult> GetRecords(){return await base.GetRecords();}
        [ProducesResponseType(typeof(Player), (int)HttpStatusCode.OK)]
        public override async Task<IActionResult> SelectRecords(string id){return await base.SelectRecords(id);}
        [ProducesResponseType(typeof(Player), (int)HttpStatusCode.OK)]
        public override async Task<IActionResult> UpsertRecord(Player record){return await base.UpsertRecord(record);}


    }
}
