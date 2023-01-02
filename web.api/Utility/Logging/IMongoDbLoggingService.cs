namespace web.api.Utility.Logging
{
    public interface IMongoDbLoggingService
    {
        Task CreateAsync(LoggingData loggingData);
        Task<List<LoggingData>> GetAsync();
        Task<LoggingData?> GetAsync(string id);
        Task RemoveAsync(string id);
        Task UpdateAsync(string id, LoggingData updateLog);
    }
}