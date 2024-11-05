using Microsoft.Extensions.DependencyInjection;
using PriceListener.Domain.Interfaces.Adapters.API.Bitstamp;

namespace PriceListener.Infrastructure.Adapters.API.Bitstamp.DependencyConfiguration
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddBitstampServices(this IServiceCollection services)
        {
            services.AddSingleton<IBitstampAdapter, BitstampAdapter>();

            return services;
        }
    }
}
