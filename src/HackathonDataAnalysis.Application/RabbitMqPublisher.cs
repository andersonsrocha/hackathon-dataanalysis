using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace HackathonDataAnalysis.Application;

public class RabbitMqPublisher(IConfiguration configuration, ILogger<RabbitMqPublisher> logger) : IRabbitMqPublisher
{
    private readonly string _hostName = configuration["RabbitMQ:HostName"] ?? "localhost";
    private readonly string _userName = configuration["RabbitMQ:UserName"] ?? "admin";
    private readonly string _password = configuration["RabbitMQ:Password"] ?? "";
    private readonly string _queueName = configuration["RabbitMQ:QueueName"] ?? "alert.queue";
    
    public async Task PublishAsync<T>(T request, CancellationToken cancellationToken = default) where T : class
    {
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

    public async Task PublishAsync<T>(string queueName, T request, CancellationToken cancellationToken = default) where T : class
    {
        logger.LogInformation("Creating a RabbitMQ Connection with HostName = {HostName}, UserName = {UserName}, Password = {Password}, QueueName = {QueueName}", _hostName, _userName, _password, queueName);
        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password,
        };

        await using var connection = await factory.CreateConnectionAsync(cancellationToken);
        logger.LogInformation("Creating a RabbitMQ Channel");
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        logger.LogInformation("Declaring queue with name: {Name}", queueName);
        await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        logger.LogInformation("Serialize request data");
        var message = JsonSerializer.Serialize(request);
        logger.LogInformation("Get bytes from message: {Message}", message);
        var body = Encoding.UTF8.GetBytes(message);
        logger.LogInformation("Sending message: {Message}", message);
        await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body, cancellationToken);
    }
}