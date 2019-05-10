using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace FeatureFlags
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var featureManager = 
                    scope.ServiceProvider.GetRequiredService<IFeatureManager>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    if(featureManager.IsEnabled(Features.EnableInformationLogs))
                        _logger.LogInformation("Worker running at: {time}", 
                            DateTimeOffset.Now);
                        
                    await Task.Delay(10000, stoppingToken);
                }
            }
        }
    }

    public class Features
    {
        public static string EnableInformationLogs => "EnableInformationLogs";
    }
}
