namespace HackathonDataAnalysis.Auth.Models;

public sealed class LoginServiceRequest
{
    public Guid ClientId { get; set; } = Guid.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}