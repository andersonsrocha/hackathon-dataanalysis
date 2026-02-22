using HackathonDataAnalysis.Auth.Interfaces;
using HackathonDataAnalysis.Auth.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HackathonDataAnalysis.Auth;

public static class AuthServiceExtensions
{
    public static void AddAuthService(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<AuthServiceOptions>()
            .Bind(configuration.GetSection("Services:AuthService"))
            .Validate(o => Uri.IsWellFormedUriString(o.BaseUrl, UriKind.Absolute), "AuthService BaseUrl invalid")
            .ValidateOnStart();
        services.AddTransient<AuthenticatedHttpClientHandler>();
        services.AddHttpClient<IAuthService, AuthService>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<AuthServiceOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.EndsWith($"/") ? options.BaseUrl : options.BaseUrl + "/");
        }).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
    }
}