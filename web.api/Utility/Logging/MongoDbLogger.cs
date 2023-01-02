using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Runtime.Versioning;
using web.api.Utility.Logging;

namespace web.api.Utility.Logging3
{

    public static class MongoDbLoggerExtensions
    {
        public static ILoggingBuilder AddMongoDbLoggerr(
            this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, MongoDbLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions<MongoDbLoggerConfiguration, MongoDbLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddMongoDbLoggerr( this ILoggingBuilder builder, Action<MongoDbLoggerConfiguration> configure)
        {
            builder.AddMongoDbLoggerr();
            builder.Services.Configure(configure);

            return builder;
        }
    }

    public class Database
    {
        public string ConnectionString { get; set; } = "";
        public string DbName { get; set; } = "";
        public string CollectionName { get; set; } = "";

    }

    public sealed class MongoDbLoggerConfiguration
    {
        public int EventId { get; set; }
        public Dictionary<string, LogLevel> logLevel { get; set; } = new();
        public Database database { get; set; } = new();      
    }

    [UnsupportedOSPlatform("browser")]
    [ProviderAlias("MongoDb")]
    public sealed class MongoDbLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable? _onChangeToken;
        private MongoDbLoggerConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, MongoDbLogger> _loggers =  new(StringComparer.OrdinalIgnoreCase);
        private readonly IMongoDbLoggingService _mongoDbLoggingService;

        public MongoDbLoggerProvider(
            IOptionsMonitor<MongoDbLoggerConfiguration> config, IServiceProvider serviceProvider)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
            _mongoDbLoggingService = serviceProvider.GetRequiredService<IMongoDbLoggingService>();
        }

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new MongoDbLogger(name, GetCurrentConfig, _mongoDbLoggingService));

        private MongoDbLoggerConfiguration GetCurrentConfig() => _currentConfig;

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken?.Dispose();
        }
    }


    public sealed class MongoDbLogger : ILogger
    {
        private readonly string _name;
        private readonly Func<MongoDbLoggerConfiguration> _getCurrentConfig;
        private readonly IMongoDbLoggingService _mongoDbLoggingService;

        public MongoDbLogger(string name, Func<MongoDbLoggerConfiguration> getCurrentConfig,IMongoDbLoggingService mongoDbLoggingService)
        {
            (_name, _getCurrentConfig, _mongoDbLoggingService) = (name, getCurrentConfig, mongoDbLoggingService);
        }
            

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            var config = _getCurrentConfig();

            if (config.logLevel.Count == 0 && logLevel >= LogLevel.Information) return true;
            if (_getCurrentConfig().logLevel.Any(x => x.Key == _name && x.Value <= logLevel)) return true;
            if (_getCurrentConfig().logLevel.Any(x => x.Key == "Default" && x.Value <= logLevel)) return true;

            return false;
        }


        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            MongoDbLoggerConfiguration config = _getCurrentConfig();
            if (config.EventId == 0 || config.EventId == eventId.Id)
            {
                var loggingData = new LoggingData();

                loggingData.Loglevel = $"{logLevel}";
                loggingData.Name = _name;
                loggingData.Message = $"{formatter(state, exception)}";
                loggingData.LogDate = DateTime.Now;

                _mongoDbLoggingService.CreateAsync(loggingData).Wait();

                //Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12}]");

                //Console.Write($" >>> Custom Logging(2) <<<  {_name} - ");

                //Console.Write($"{formatter(state, exception)}");
            }
        }
    }
}
