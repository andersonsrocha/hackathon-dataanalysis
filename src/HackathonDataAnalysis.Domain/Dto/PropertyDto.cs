namespace HackathonDataAnalysis.Domain.Dto;

public record PropertyDto(Guid Id, bool Active, string Name, double Size, Guid OwnerId, ICollection<PlotDto> Plots);