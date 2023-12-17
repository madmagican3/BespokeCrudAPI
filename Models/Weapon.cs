using BasicCrudAPI.Models.BaseModels;

namespace BasicCrudAPI.Models
{
    public class Weapon : BaseDatabaseModel
    {
        public string Name { get; set; }
        public string Effect { get; set; }

    }
}
