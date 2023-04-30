using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace QuartzJobWithIOC
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var services = new ServiceCollection();
            // Enregistrer le job factory dans le conteneur IOC
            services.AddSingleton<IJobFactory, MyJobFactory>();
            // Enregistrer le scheduler factory dans le conteneur IOC
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            // Enregistrer le scheduler dans le conteneur IOC
            services.AddSingleton(async sp =>
            {
                var schedulerFactory = sp.GetRequiredService<ISchedulerFactory>();
                var jobFactory = sp.GetRequiredService<IJobFactory>();
                var scheduler = await schedulerFactory.GetScheduler();
                scheduler.JobFactory = jobFactory;
                await scheduler.Start();
                return scheduler;
            });
            // Enregistrer le job dans le conteneur IOC
            services.AddSingleton<MyJob>();
            services.AddMyService();
          
        }
    }

    public class MyJob : IJob
    {
        private readonly IMyService _myService;

        public MyJob(IMyService myService)
        {
            _myService = myService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _myService.DoSomething();
        }
    }

 
    public class MyJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MyJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProvider.GetService<MyJob>();
        }

        public void ReturnJob(IJob job)
        {
            // Rien à faire ici
        }
    }

    public interface IMyService
    {
        Task DoSomething();
    }

    public class MyService : IMyService
    {
        public async Task DoSomething()
        {
            Console.WriteLine("MyService is doing something...");
            await Task.Delay(1000);
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyService(this IServiceCollection services)
        {
            services.AddTransient<IMyService, MyService>();
            return services;
        }
        public static IServiceCollection AddQuartz(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, MyJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton(async sp =>
            {
                var schedulerFactory = sp.GetRequiredService<ISchedulerFactory>();
                var jobFactory = sp.GetRequiredService<IJobFactory>();
                var scheduler = await schedulerFactory.GetScheduler();
                scheduler.JobFactory = jobFactory;
                await scheduler.Start();
                return scheduler;
            });

            return services;
        }
    }
    
}
