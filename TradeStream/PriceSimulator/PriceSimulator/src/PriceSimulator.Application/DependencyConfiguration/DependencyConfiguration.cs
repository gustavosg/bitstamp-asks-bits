using Microsoft.Extensions.DependencyInjection;
using PriceSimulator.Application.Interfaces;
using PriceSimulator.Application.Services;

namespace PriceSimulator.Application.DependencyConfiguration
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPriceService, PriceService>();
            return services;
        }
    }
}
