using Quartz;
using Quartz.Spi;

public class JobFactory : IJobFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public JobFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var job = ActivatorUtilities.CreateInstance(scope.ServiceProvider, bundle.JobDetail.JobType) as IJob;
        return job;
    }

    public void ReturnJob(IJob job)
    {
        var disposable = job as IDisposable;
        disposable?.Dispose();
    }
}