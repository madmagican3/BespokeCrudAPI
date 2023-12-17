namespace BasicCrudAPI.Models.BaseModels
{
    public class BaseDatabaseModel
    {
        public string id { get; set; } 
        public string PartitionKey { get; set; } 
        public bool Active { get; set; } = true;
    }
}
