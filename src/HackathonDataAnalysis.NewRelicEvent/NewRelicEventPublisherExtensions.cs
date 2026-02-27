using HackathonDataAnalysis.NewRelicEvent.Interfaces;
using HackathonDataAnalysis.NewRelicEvent.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HackathonDataAnalysis.NewRelicEvent;

public static class NewRelicEventPublisherExtensions
{
    public static void AddNewRelicEventPublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<NewRelicEventPublisherOptions>()
            .Bind(configuration.GetSection("Services:NewRelic"))
            .Validate(o => Uri.IsWellFormedUriString(o.BaseUrl, UriKind.Absolute), "NewRelic BaseUrl inválida")
            .ValidateOnStart();
        services.AddTransient<AuthenticatedHttpClientHandler>();
        services.AddHttpClient<INewRelicEventPublisher, NewRelicEventPublisher>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<NewRelicEventPublisherOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.EndsWith($"/") ? options.BaseUrl : options.BaseUrl + "/");
            client.DefaultRequestHeaders.Add("Api-Key", options.ApiKey);
        }).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
    }
}