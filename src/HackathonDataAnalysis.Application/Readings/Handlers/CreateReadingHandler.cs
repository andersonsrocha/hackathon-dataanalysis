using System.Text;
using System.Text.Json;
using AutoMapper;
using HackathonDataAnalysis.Application.Readings.Commands;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Enums;
using HackathonDataAnalysis.Domain.Interfaces;
using HackathonDataAnalysis.Domain.Models;
using HackathonDataAnalysis.Plots.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OperationResult;
using RabbitMQ.Client;

namespace HackathonDataAnalysis.Application.Readings.Handlers;

public class CreateReadingHandler(IReadingRepository repository, IRuleRepository ruleRepository, IPlotService plotService, IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateReadingHandler> logger) : IRequestHandler<CreateReadingRequest, Result<ReadingDto>>
{
    private readonly string _hostName = configuration["RabbitMQ:HostName"] ?? "localhost";
    private readonly string _userName = configuration["RabbitMQ:UserName"] ?? "admin";
    private readonly string _password = configuration["RabbitMQ:Password"] ?? "";
    private readonly string _queueName = configuration["RabbitMQ:QueueName"] ?? "alert.queue";
    
    public async Task<Result<ReadingDto>> Handle(CreateReadingRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new reading with plot: {PlotId}", request.PlotId);
        
        logger.LogInformation("Checking if plot with id {PlotId} exists", request.PlotId);
        var plot = await plotService.FindAsync(request.PlotId, cancellationToken);
        if (plot is null)
            return Result.Error<ReadingDto>(new Exception("Plot not found"));

        var reading = (Reading)request;
        repository.Add(reading);
        
        logger.LogInformation("Evaluating rules for the new reading.");
        var rules = await ruleRepository.FindByPlotAsync(reading.PlotId, cancellationToken);
        foreach (var rule in rules)
        {
            var since = DateTime.UtcNow.AddMinutes(-rule.DurationMinutes);
            var readings = await repository.FindByPlotAndSinceAsync(reading.PlotId, since, cancellationToken);
            var value = readings.Average(r => GetSensorValue(r, rule.SensorType));
            if (!Evaluate(value, rule)) continue;
            
            logger.LogInformation("Creating a new alert for plot: {PlotId}", request.PlotId);
            logger.LogInformation("Creating a RabbitMQ Connection with HostName = {HostName}, UserName = {UserName}, Password = {Password}, QueueName = {QueueName}", _hostName, _userName, _password, _queueName);
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password,
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            logger.LogInformation("Creating a RabbitMQ Channel");
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
            logger.LogInformation("Declaring queue with name: {Name}", _queueName);
            await channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
            logger.LogInformation("Serialize request data");
            var message = JsonSerializer.Serialize(request);
            logger.LogInformation("Get bytes from message: {Message}", message);
            var body = Encoding.UTF8.GetBytes(message);
            logger.LogInformation("Sending message: {Message}", message);
            await channel.BasicPublishAsync(exchange: "", routingKey: _queueName, body: body, cancellationToken);
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