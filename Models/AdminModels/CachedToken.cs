using BasicCrudAPI.Annotations;
using BasicCrudAPI.Models.BaseModels;

namespace BasicCrudAPI.Models.AdminModels
{
    [TTL(15)]
    public class Token : BaseDatabaseModel
    {
        public string UserId { get; set; }
        public string AuthorizationToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
