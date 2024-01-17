using Microsoft.Extensions.DependencyInjection;
using Rainfall.Core.FloodMonitoring;

namespace Rainfall.Core
{
    public static class ConfigurationServices
    {
        public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IFloodMonitoringService, FloodMonitoringService>();
            return services;
        }
    }
}
