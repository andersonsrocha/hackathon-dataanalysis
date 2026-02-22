using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Enums;
using HackathonDataAnalysis.Domain.Models;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Rules.Commands;

public sealed class CreateRuleRequest : IRequest<Result<RuleDto>>
{
    public Guid PlotId { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public SensorType SensorType { get; set; } = SensorType.None;
    public Operator Operator { get; set; } = Operator.None;
    public double Threshold { get; set; }
    public int DurationMinutes { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    
    public static implicit operator Rule(CreateRuleRequest request)
        => new(request.PlotId, request.Name, request.SensorType, request.Operator, request.Threshold, request.DurationMinutes, request.Status, request.Message);
}