using BasicCrudAPI.Models.BaseModels;

namespace BasicCrudAPI.Models.AdminModels
{
    public class User : BaseDatabaseModel
    {
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsApproved { get; set; }
        public string UserEmail { get; set; }
        public string PasswordHash { get; set; }
    }
}
