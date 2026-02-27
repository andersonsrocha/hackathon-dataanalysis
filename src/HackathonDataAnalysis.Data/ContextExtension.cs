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
    public static void AddMongoClient(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration.GetConnectionString("MongoConnection") ?? throw new InvalidOperationException("MongoConnection string is not configured."));
        services.AddSingleton<IMongoClient>(mongoClient);
    }
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

    public static void AddMongoContext(this IServiceCollection services)
    {
        services.AddDbContext<HackathonDataAnalysisMongoContext>((sp, options) => options.UseMongoDB(sp.GetRequiredService<IMongoClient>(), "hackathon"));
    }
    
    public static void AddSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HackathonDataAnalysisSqlContext>(options => options.UseSqlServer(configuration.GetConnectionString("SQLConnection")) );
    }
}