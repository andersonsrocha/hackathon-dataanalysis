using HackathonDataAnalysis.Domain.Dto;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Readings.Commands;

public sealed class UpdateReadingRequest : IRequest<Result<ReadingDto>>
{
    public Guid Id { get; init; } = Guid.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public double SoilMoisture { get; set; } = 0.0;
    public double Temperature { get; set; } = 0.0;
    public double Precipitation { get; set; } = 0.0;
}