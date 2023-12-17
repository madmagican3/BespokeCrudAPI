using System.Reflection;
using System.Text.Json;
using BasicCrudAPI.Annotations;
using BasicCrudAPI.Models.BaseModels;
using Microsoft.Azure.Cosmos;

namespace BasicCrudAPI.Classes
{
    public class DbHelper
    {
        private IConfiguration config { get; set; }
        public DbHelper(IConfiguration config)
        {
            this.config = config;
        }
        private readonly string DatabaseName = "Dream";
        private Database _database { get; set; } = null;
        private CosmosClient _client { get; set; } = null;
        public Container GetContainer(string containerName, int ttl = -1)
        {
            _client ??= new CosmosClient(config.GetConnectionString("CosmosConnectionString"));
            var databaseCreateResponse = _client.CreateDatabaseIfNotExistsAsync(DatabaseName).Result;
            _database ??= _client.GetDatabase(DatabaseName);

            var containerCreateResponse = _database.CreateContainerIfNotExistsAsync(new ContainerProperties(containerName, $"/{nameof(BaseDatabaseModel.PartitionKey)}")
            {
                DefaultTimeToLive = ttl
            }).Result;
            return _database.GetContainer(containerName);
        }
        public Container GetContainer<T>()
        {
            return GetContainer(typeof(T).Name, typeof(T).GetCustomAttribute<TTLAttribute>() != null ? (typeof(T).GetCustomAttribute<TTLAttribute>().TTLInMinutes*60):-1);
        }
        public IEnumerable<T> GetRecords<T>() where T : BaseDatabaseModel
        {
            return GetContainer<T>()
                .GetItemLinqQueryable<T>(true)
                .Where(x => x.Active)
                .AsEnumerable();
        }
        public IEnumerable<T> GetRecords<T>(Func<T, bool> predicate) where T : BaseDatabaseModel
        {
            return GetContainer<T>()
                .GetItemLinqQueryable<T>(true)
                .Where(predicate)
                .Where(x => x.Active)
                .AsEnumerable();
        }
        public T? SelectRecord<T>(string id) where T : BaseDatabaseModel
        {
            var result = GetContainer<T>().GetItemLinqQueryable<T>(true)
                .Where(x => x.Active && x.id == id)
                .AsEnumerable();
            return result.FirstOrDefault();
        }
        public T UpsertRecord<T>(T record) where T : BaseDatabaseModel
        {
            record.id ??= Guid.NewGuid().ToString();
            record.PartitionKey ??= "IDunno";
            var result = GetContainer<T>().UpsertItemAsync(record).Result;
            return record;
        }
    }
}
