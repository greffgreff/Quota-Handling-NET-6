using quota.Quota;

namespace quota.Services
{
    public class WeatheForecastService
    {
        private readonly QuotaManager _quota;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatheForecastService()
        {
            _quota = QuotaManager.Of(GetType().Name, 10, TimeSpan.FromSeconds(10));
        }

        public IEnumerable<WeatherForecast> Get()
        {
            return _quota.CanUse() 
                ? Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray() 
                : Array.Empty<WeatherForecast>();
        }
    }
}
