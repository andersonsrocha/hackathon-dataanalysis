using HackathonDataAnalysis.Domain.Models;

namespace HackathonDataAnalysis.NewRelicEvent.Interfaces;

public interface INewRelicEventPublisher
{
    Task PublishReadingEventAsync(Reading reading, CancellationToken cancellationToken);
}