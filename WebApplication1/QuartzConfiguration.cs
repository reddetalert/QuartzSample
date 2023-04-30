using Quartz;

public class QuartzConfiguration
{
    public void Configure(ISchedulerFactory schedulerFactory, IServiceScopeFactory serviceScopeFactory)
    {
        var scheduler = schedulerFactory.GetScheduler().Result;
        scheduler.JobFactory = new JobFactory(serviceScopeFactory);

        var job = JobBuilder.Create<SimpleJob>()
            .WithIdentity("SimpleJob")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity("SimpleJobTrigger","group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithInterval(TimeSpan.FromSeconds(10))
                .RepeatForever())
            .Build();

        scheduler.ScheduleJob(job, trigger).Wait();
        scheduler.Start().Wait();
    }
}