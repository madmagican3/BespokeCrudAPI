using BasicCrudAPI.Models.BaseModels;

namespace BasicCrudAPI.Models
{
    public class Player : BaseDatabaseModel
    {
        public bool IsApproved { get; set; }
        public bool IsAlive { get; set; }

    }
}
