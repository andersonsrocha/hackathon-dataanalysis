using HackathonDataAnalysis.Plots.Interfaces;
using HackathonDataAnalysis.Plots.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HackathonDataAnalysis.Plots;

public static class PlotServiceExtensions
{
    public static void AddPlotService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CredentialsOptions>(configuration.GetSection("Credentials"));
        services
            .AddOptions<PlotServiceOptions>()
            .Bind(configuration.GetSection("Services:PlotService"))
            .Validate(o => Uri.IsWellFormedUriString(o.BaseUrl, UriKind.Absolute), "PlotService BaseUrl inválida")
            .ValidateOnStart();
        services.AddTransient<AuthenticatedHttpClientHandler>();
        services.AddHttpClient<IPlotService, PlotService>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<PlotServiceOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.EndsWith($"/") ? options.BaseUrl : options.BaseUrl + "/");
        }).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
    }
}