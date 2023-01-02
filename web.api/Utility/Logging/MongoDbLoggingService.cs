using Microsoft.Extensions.Options;
using MongoDB.Driver;
using web.api.Appsettings;
using web.api.Utility.Logging3;

namespace web.api.Utility.Logging
{
    public class MongoDbLoggingService : IMongoDbLoggingService
    {
        private readonly IMongoCollection<LoggingData> _loggingsCollection;
        public MongoDbLoggingService(IOptionsMonitor<MongoDbLoggerConfiguration> config)
        {
            var mongoClient = new MongoClient(config.CurrentValue.database.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.CurrentValue.database.DbName);
            _loggingsCollection = mongoDatabase.GetCollection<LoggingData>(config.CurrentValue.database.CollectionName);
        }

        public async Task<List<LoggingData>> GetAsync() =>
            await _loggingsCollection.Find(_ => true).ToListAsync();

        public async Task<LoggingData?> GetAsync(string id) =>
            await _loggingsCollection.Find(x => x.UserId == id).FirstOrDefaultAsync();

        public async Task CreateAsync(LoggingData loggingData) =>
            await _loggingsCollection.InsertOneAsync(loggingData);

        public async Task UpdateAsync(string id, LoggingData updateLog) =>
            await _loggingsCollection.ReplaceOneAsync(x => x.UserId == id, updateLog);

        public async Task RemoveAsync(string id) =>
            await _loggingsCollection.DeleteOneAsync(x => x.UserId == id);

    }
}
