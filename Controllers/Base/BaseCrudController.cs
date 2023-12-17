using System.Reflection;
using BasicCrudAPI.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.OpenApi.Validations.Rules;

namespace BasicCrudAPI.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseCrudController<T> : BaseController where T : BaseDatabaseModel
    {
        protected BaseCrudController(IConfiguration config) : base(config) { }

        [HttpGet("Get")]
        public virtual async Task<IActionResult> GetRecords()
        {
            return Ok(_dbHelper.GetRecords<T>());
        }

        [HttpGet("select/{id}")]
        public virtual async Task<IActionResult> SelectRecords(string id)
        {
            return Ok(_dbHelper.SelectRecord<T>(id));
        }
        [HttpPost("Upsert")]
        public virtual async Task<IActionResult> UpsertRecord(T record)
        {
            return Ok(_dbHelper.UpsertRecord(record));
        }

    }
}
