using System.Text;
using System.Text.Json;
using AutoMapper;
using HackathonDataAnalysis.Application.Readings.Commands;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Enums;
using HackathonDataAnalysis.Domain.Interfaces;
using HackathonDataAnalysis.Domain.Models;
using HackathonDataAnalysis.NewRelicEvent.Interfaces;
using HackathonDataAnalysis.Plots.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OperationResult;
using RabbitMQ.Client;

namespace HackathonDataAnalysis.Application.Readings.Handlers;

public class CreateReadingHandler(IReadingRepository repository, IRuleRepository ruleRepository, IPlotService plotService, IRabbitMqPublisher bus, INewRelicEventPublisher publisher, IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateReadingHandler> logger) : IRequestHandler<CreateReadingRequest, Result<ReadingDto>>
{
    private readonly string _queueName = configuration["RabbitMQ:QueueName"] ?? "alert.queue";
    
    public async Task<Result<ReadingDto>> Handle(CreateReadingRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new reading with plot: {PlotId}", request.PlotId);
        
        logger.LogInformation("Checking if plot with id {PlotId} exists", request.PlotId);
        var plot = await plotService.FindAsync(request.PlotId, cancellationToken);
        if (plot is null)
            return Result.Error<ReadingDto>(new Exception("Plot not found"));

        request.PlotName = plot.Name;

        var reading = (Reading)request;
        repository.Add(reading);
        
        logger.LogInformation("Sending reading with id {Guid} to New Relic", reading.Id);
        await publisher.PublishReadingEventAsync(reading, cancellationToken);
        
        logger.LogInformation("Evaluating rules for the new reading.");
        var rules = await ruleRepository.FindByPlotAsync(reading.PlotId, cancellationToken);
        foreach (var rule in rules)
        {
            var since = DateTime.UtcNow.AddMinutes(-rule.DurationMinutes);
            var readings = await repository.FindByPlotAndSinceAsync(reading.PlotId, since, cancellationToken);
            var value = readings.Select(r => GetSensorValue(r, rule.SensorType)).DefaultIfEmpty(0).Average();
            if (!Evaluate(value, rule)) continue;
            
            logger.LogInformation("Creating a new alert for plot: {PlotId}", request.PlotId);
            var alert = new AlertDto(plot.Id, plot.Name, rule.Status, rule.Message);
            await bus.PublishAsync(_queueName, alert, cancellationToken);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
        logger.LogInformation("Reading created successfully.");
        
        return Result.Success(mapper.Map<ReadingDto>(reading));
    }
    
    private static double GetSensorValue(Reading reading, SensorType sensorType)
    {
        return sensorType switch
        {
            SensorType.SoilMoisture => reading.SoilMoisture,
            SensorType.Temperature => reading.Temperature,
            SensorType.Precipitation => reading.Precipitation,
            SensorType.None => 0.0,
            _ => throw new ArgumentException($"Unknown sensor type: {sensorType}")
        };
    }

    private static bool Evaluate(double value, Rule rule)
    {
        return rule.Operator switch
        {
            Operator.GreaterThan => value > rule.Threshold,
            Operator.LessThan => value < rule.Threshold,
            Operator.EqualTo => Math.Abs(value - rule.Threshold) < 0.0001,
            Operator.NotEqualTo => Math.Abs(value - rule.Threshold) >= 0.0001,
            Operator.GreaterThanOrEqualTo => value >= rule.Threshold,
            Operator.LessThanOrEqualTo => value <= rule.Threshold,
            Operator.None => false,
            _ => throw new ArgumentException($"Unknown operator: {rule.Operator}")
        };
    }
}