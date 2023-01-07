using Microsoft.AspNetCore.Mvc;
using quota.Services;

namespace quota.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatheForecastService _service;

        public WeatherForecastController(WeatheForecastService service) 
        {
            _service = service;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return _service.Get();
        }
    }
}