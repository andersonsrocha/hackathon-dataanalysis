using Microsoft.AspNetCore.Http;

namespace HackathonDataAnalysis.Auth;

public class AuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var correlationId = httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();
        request.Headers.Add("X-Correlation-ID", correlationId);

        return await base.SendAsync(request, cancellationToken);
    }
}
