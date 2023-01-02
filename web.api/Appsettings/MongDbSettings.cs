using MongoDB.Driver.Core.Configuration;

namespace web.api.Appsettings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = "";
        public string DatabaseName { get; set; } = "";
        public string CollectionName { get; set; } = "";

    }
}
