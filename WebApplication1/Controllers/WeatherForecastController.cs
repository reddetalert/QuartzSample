using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISchedulerFactory _schedulerFactory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISchedulerFactory schedulerFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("ExecuteJob", Name = "Execute job")]
        public async Task<IActionResult> ExecuteJob()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var job = JobBuilder.Create<SimpleJob>()
                .WithIdentity("name", "group")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("name", "group")
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(job, trigger);


            // if (!await scheduler.CheckExists(triggerKey))
            // {
            //     return NotFound();
            // }
            //var trigger =  await scheduler.GetTrigger(triggerKey);
            // await scheduler.TriggerJob(trigger.JobKey);
            return Ok();
        }
    }
}