using HackathonDataAnalysis.Domain.Enums;

namespace HackathonDataAnalysis.Domain.Dto;

public record RuleDto(Guid Id, bool Active, Guid PlotId, string Name, SensorType SensorType, Operator Operator, double Threshold, int DurationMinutes, string Status, string Message);