using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Models;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Readings.Commands;

public sealed class CreateReadingRequest : IRequest<Result<ReadingDto>>
{
    public Guid PlotId { get; set; } = Guid.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public double SoilMoisture { get; set; } = 0.0;
    public double Temperature { get; set; } = 0.0;
    public double Precipitation { get; set; } = 0.0; 
    
    public static implicit operator Reading(CreateReadingRequest request)
        => new(request.PlotId, request.Date, request.SoilMoisture, request.Temperature, request.Precipitation);
}