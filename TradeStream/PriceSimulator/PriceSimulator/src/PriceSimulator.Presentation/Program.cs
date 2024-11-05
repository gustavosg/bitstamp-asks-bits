using PriceSimulator.Application.DependencyConfiguration;
using Microsoft.AspNetCore.Authentication.Certificate;
using PriceSimulator.Application.Interfaces;
using PriceSimulator.Application.Services;
using PriceSimulator.Infrastructure.Adapters.Persistence.MySQL.DependencyConfiguration;
using System.Runtime.CompilerServices;
using System.Threading;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication(
          CertificateAuthenticationDefaults.AuthenticationScheme)
          .AddCertificate();


        //services.AddSingleton(cancellationTokenSource);
        //services.AddDbRepositories();
        builder.Services.AddMySqlServices();
        builder.Services.AddDbRepositories();
        builder.Services.AddServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}