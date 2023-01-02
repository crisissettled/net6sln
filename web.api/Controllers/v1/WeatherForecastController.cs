using infrustruture;
using Microsoft.AspNetCore.Mvc;
using web.api.Utility;

namespace web.api.v1.Controllers {
    //[ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = nameof(ApiVersions.v1))]
    public class WeatherForecastController : ApiControllerBaseV1 { 
    
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger) {
            _logger = logger;
        }

        /// <summary>
        /// get weather info
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetWeatherForecast")]
        public string Get() {
            return  "new from master";
        }

        [HttpPost(Name = "File")]
        public JsonResult UploadFile(IFormCollection form) {
            return new JsonResult(new { success = true, message = "success" });
        }

        [HttpGet]
        [Route("{day:int}")]
        public string GetDay(int day) {
            return $"your day { day }";
        }

        [HttpPut]
        public string update() {
            return "fffffaaabbeee";
        }


        [HttpDelete]
        public string DeleteInfo([FromServices] IShowUserInfo showUserInfo) {

            return showUserInfo.ShowInfo();
        }
    }
}