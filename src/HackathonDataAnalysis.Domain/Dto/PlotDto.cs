namespace HackathonDataAnalysis.Domain.Dto;

public record PlotDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public Guid PropertyId { get; set; } = Guid.Empty;
};