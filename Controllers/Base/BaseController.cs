using BasicCrudAPI.Classes;
using BasicCrudAPI.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace BasicCrudAPI.Controllers.Base
{
    public class BaseController : ControllerBase 
    {
      
        public DbHelper _dbHelper { get; }
        public BaseController(IConfiguration config)
        {
           _dbHelper = new DbHelper(config);
        }

    }
}
