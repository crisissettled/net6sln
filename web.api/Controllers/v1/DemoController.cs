using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using web.api.Model;
using web.api.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web.api.Controllers.v1
{
    public class DemoController : ApiControllerBaseV1
    {
        private readonly ILogger<DemoController> _logger;
        private readonly IMemoryCache _memoryCache;
        public DemoController(ILogger<DemoController> logger,IMemoryCache memoryCache)
        {
            _logger= logger;
            _memoryCache= memoryCache;
        }

        //[ResponseCache(Duration = 10)]
        [HttpGet]
        public string Get(string name,int age)
        {
           var result =  _memoryCache.GetOrCreate($"{name}", e =>
            {
                //e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                //e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Random.Shared.Next(10,15));
                double x = Random.Shared.NextDouble();
                double min = 10.0;
                double max = 15.0;               
                double r = x * max + (1 - x)* min;

                var r1 = 10 / Convert.ToInt32(name);
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(r);
                return $"{name} - {DateTime.Now} - {Environment.TickCount64}";
            });


            return result; //$"{name} - {age}  + {DateTime.Now.ToLongTimeString()}";

        }
        //[HttpGet]
        //public IEnumerable<string> Get([FromServices] IOptions<DbOptions> options, [FromServices] IOptionsMonitor<DbOptions> optionsMonitor
        //    ,[FromServices] IOptionsSnapshot<DbOptions> optionsSnapshot)
        //{
        //    DbOptions _dbOptions = options.Value;
        //    DbOptions _dbOptionsMonitor = optionsMonitor.CurrentValue;
        //    DbOptions _dbOptionsSnapshot = optionsSnapshot.Value;
        //    return new string[] {
        //        _dbOptions.Datebase, _dbOptions.UserID,
        //        _dbOptionsMonitor.Datebase, _dbOptionsMonitor.UserID,
        //        _dbOptionsSnapshot.Datebase,_dbOptionsSnapshot.UserID
        //    };
        //}

        // GET api/<DemoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogInformation(new Exception("testing error"),$"info---> {id}");
            _logger.LogDebug($"debug---> {id}");
            _logger.LogWarning(new Exception("testing error2"),$"warning---> {id}");
            _logger.LogError(new Exception("testing error3"),$"error---> {id}");
            _logger.LogCritical($"critical---> {id}");
            return $"Your input {id} @ {DateTime.Now.ToLongTimeString()}";
        }

        // POST api/<DemoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DemoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DemoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
