using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BasicCrudAPI.Annotations;
using BasicCrudAPI.Controllers.Base;
using BasicCrudAPI.Models;

namespace BasicCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponController : BaseCrudController<Weapon>
    {
        public WeaponController(IConfiguration config) : base(config) { }
        [ProducesResponseType(typeof(IEnumerable<Weapon>), (int)HttpStatusCode.OK)]
        public override async Task<IActionResult> GetRecords() { return await base.GetRecords(); }
        [ProducesResponseType(typeof(Weapon), (int)HttpStatusCode.OK)]
        public override async Task<IActionResult> SelectRecords(string id) { return await base.SelectRecords(id); }
        [ProducesResponseType(typeof(Weapon), (int)HttpStatusCode.OK)]
        [Admin]
        public override async Task<IActionResult> UpsertRecord(Weapon record) { return await base.UpsertRecord(record); }
    }
}
