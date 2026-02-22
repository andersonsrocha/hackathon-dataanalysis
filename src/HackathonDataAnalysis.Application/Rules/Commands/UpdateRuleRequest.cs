using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Enums;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Rules.Commands;

public sealed class UpdateRuleRequest : IRequest<Result<RuleDto>>
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public SensorType SensorType { get; set; } = SensorType.None;
    public Operator Operator { get; set; } = Operator.None;
    public double Threshold { get; set; }
    public int DurationMinutes { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}