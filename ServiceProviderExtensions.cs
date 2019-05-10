using System;
using Microsoft.Extensions.DependencyInjection;

namespace FeatureFlags
{
    public static class ServiceProviderExtensions
    {
        public static void UseScopedService<T>(this IServiceProvider serviceProvider, 
            Action<T> action)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<T>();
                action(service);
            }
        }
    }
}