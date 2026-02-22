namespace HackathonDataAnalysis.Plots;

public class CredentialsOptions
{
    public Guid ClientId { get; set; } = Guid.Empty;
    public string ClientSecret { get; set; } = null!;
}