using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Seeds;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                //var logger = loggerFactory.CreateLogger("SeedLogger");
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var appDbContext = services.GetRequiredService<ApplicationDbContext>();

                    await appDbContext.Database.MigrateAsync();

                    await Infrastructure.Identity.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
                    await Infrastructure.Identity.Seeds.DefaultSupport.SeedAsync(userManager, roleManager);
                    await Infrastructure.Identity.Seeds.DefaultBasicUser.SeedAsync(userManager, roleManager);

                    await DefaultUserProfile.SeedAsync(appDbContext);
                    await DefaultRestaurants.SeedAsync(appDbContext);
                    await DefaultMenuItems.SeedAsync(appDbContext);
                    await DefaultCouriers.SeedAsync(appDbContext);
                    await DefaultFiles.SeedAsync(appDbContext);
                    await DefaultComments.SeedAsync(appDbContext);
                    
                    //await DefaultAdminUserProfile.SeedAsync(appDbContext);


                    //Log.Information("Finished Seeding Default Data");
                    //Log.Information("Application Starting");
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "An error occurred seeding the DB");
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog() //Uses Serilog instead of default .NET Logger
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        //public static IWebHost BuildWebHost(string[] args)
        //{
        //    return WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .UseKestrel(options =>
        //        {
        //            options.Limits.MaxRequestBodySize = long.MaxValue;
        //        })
        //        .UseIISIntegration()
        //        .Build();
        //}

    }

}
