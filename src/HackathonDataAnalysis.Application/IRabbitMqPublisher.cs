namespace HackathonDataAnalysis.Application;

public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(T request, CancellationToken cancellationToken = default) where T : class;
    Task PublishAsync<T>(string queueName, T request, CancellationToken cancellationToken = default) where T : class;
}