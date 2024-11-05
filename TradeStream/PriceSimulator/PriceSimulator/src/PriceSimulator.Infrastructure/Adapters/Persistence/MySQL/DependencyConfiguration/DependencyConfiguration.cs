using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PriceSimulator.Domain.Interfaces.Persistence.MySQL;
using System.Reflection;

namespace PriceSimulator.Infrastructure.Adapters.Persistence.MySQL.DependencyConfiguration
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddMySqlServices(this IServiceCollection services)
        {
            string connectionString = "Server=localhost;Database=b3-digitas-db;User=dev;Password=1234;";

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString,
                    new MySqlServerVersion(new Version(8, 0, 21)),
                    opt =>
                    {
                        opt.MigrationsAssembly(
                            Assembly.Load(Assembly.GetExecutingAssembly().ToString()).FullName
                            );
                    });
            });

            services.AddScoped<DbContext>();
            services.AddScoped<AppDbContext>();
            return services;
        }

        public static IServiceCollection AddDbRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderBookRepository, OrderBookRepository>();
            services.AddScoped<IPriceSimulationRepository, PriceSimulationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
