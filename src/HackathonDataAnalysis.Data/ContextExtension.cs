using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using HackathonDataAnalysis.Data.Repositories;
using HackathonDataAnalysis.Domain.Interfaces;
using MongoDB.Driver;

namespace HackathonDataAnalysis.Data;

public static class ContextExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.Scan(scan => scan
            .FromAssemblies(typeof(Repository<>).Assembly, typeof(IRepository<>).Assembly)
            .AddClasses(c => c.AssignableTo(typeof(IRepository<>)))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }

    public static void AddMongoContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HackathonDataAnalysisContext>(options =>
        {
            var mongoClient = new MongoClient((string)configuration.GetConnectionString("MongoConnection"));
            options.UseMongoDB(mongoClient, "Hackathon");
        });
    }
}