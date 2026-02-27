namespace HackathonDataAnalysis.Domain.Dto;

public record ReadingDto(Guid Id, bool Active, Guid PlotId, string PlotName, DateTime Date, double SoilMoisture, double Temperature, double Precipitation);