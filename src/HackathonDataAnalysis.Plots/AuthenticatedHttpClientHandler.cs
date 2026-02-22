using System.Net.Http.Headers;
using HackathonDataAnalysis.Auth.Interfaces;
using HackathonDataAnalysis.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HackathonDataAnalysis.Plots;

public class AuthenticatedHttpClientHandler(IAuthService authService, IOptions<CredentialsOptions> options, IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var payload = new LoginServiceRequest
        {
            ClientId = options.Value.ClientId,
            ClientSecret = options.Value.ClientSecret,
        };
        var token = await authService.Token(payload, cancellationToken);
        if (token is not null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var correlationId = httpContextAccessor.HttpContext?.Items["correlationId"]?.ToString() ?? Guid.NewGuid().ToString();
        request.Headers.Add("X-Correlation-ID", correlationId);

        return await base.SendAsync(request, cancellationToken);
    }
}
