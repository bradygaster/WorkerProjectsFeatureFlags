using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

namespace FeatureFlags
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var settings = config.Build();
                    config.AddAzureAppConfiguration(options => 
                    {
                        var connectionString = settings["ConnectionStrings:AppConfig"];
                        options
                            .Connect(connectionString)
                            .Watch(Features.EnableInformationLogs, TimeSpan.FromMinutes(1))
                            .UseFeatureFlags();
                    });
                })
                .ConfigureServices(services =>
                {
                    services.AddFeatureManagement();
                    services.AddHostedService<Worker>();
                });
    }
}
