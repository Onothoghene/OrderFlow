using Application.Behaviours;
using Application.Helper;
using Application.JobServices;
using FluentValidation;
using Hangfire;
using Hangfire.InMemory;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => { } ,Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(ser => ser.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            //services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMemoryCache();

            //hangfire
            //var options = new SqlServerStorageOptions
            //{
            //    UseRecommendedIsolationLevel = true,
            //    QueuePollInterval = TimeSpan.FromSeconds(15),
            //    JobExpirationCheckInterval = TimeSpan.FromHours(1),
            //    CountersAggregateInterval = TimeSpan.FromMinutes(5),
            //    PrepareSchemaIfNecessary = true,
            //    DashboardJobListLimit = 50000,
            //    TransactionTimeout = TimeSpan.FromMinutes(1),
            //};

           // var sqlStorage = new SqlServerStorage(configuration.GetConnectionString("DefaultConnection"), options);
            JobStorage.Current = new InMemoryStorage();

            services.AddHangfire(x =>
            x.UseInMemoryStorage()
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings());

            services.AddHangfireServer();

            //services.AddTransient<IStageHelper, StageHelper>();
            services.AddTransient<ILogWriter, LogWriter>();

            services.AddScoped<IPeriodicLoginService, PeriodicLoginService>();
            services.AddScoped<IOrderCompletionJobService, OrderCompletionJob>();

        }

        public static void UseHangfireDashboard(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/mydashboard");
        }

        public static void AddBackgroundJobs(this IServiceCollection services)
        {
            //cron job expressions

            RecurringJob.AddOrUpdate<IOrderCompletionJobService>("Order Completion", service => service.ProcessPendingOrders(),
            "*/15 * * * *"); // Runs every 15 minutes

        }
    }
}
