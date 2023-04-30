using Quartz;

namespace WebApplication1
{
    public class QuartzHostedService: BackgroundService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly QuartzConfiguration _quartzConfiguration;

        public QuartzHostedService(ISchedulerFactory schedulerFactory, IServiceScopeFactory serviceScopeFactory, QuartzConfiguration quartzConfiguration)
        {
            _schedulerFactory = schedulerFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _quartzConfiguration = quartzConfiguration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scheduler = await _schedulerFactory.GetScheduler(stoppingToken);
            _quartzConfiguration.Configure(_schedulerFactory, _serviceScopeFactory);
        }

    }
}
