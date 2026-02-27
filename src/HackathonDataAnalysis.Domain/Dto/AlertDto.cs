namespace HackathonDataAnalysis.Domain.Dto;

public record AlertDto(Guid PlotId, string PlotName, string Status, string Message);