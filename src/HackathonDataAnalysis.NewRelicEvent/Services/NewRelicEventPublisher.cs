using System.Text;
using System.Text.Json;
using HackathonDataAnalysis.Domain.Models;
using HackathonDataAnalysis.NewRelicEvent.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackathonDataAnalysis.NewRelicEvent.Services;

public class NewRelicEventPublisher(HttpClient httpClient, IOptions<NewRelicEventPublisherOptions> options, ILogger<NewRelicEventPublisher> logger) : INewRelicEventPublisher
{
    public async Task PublishReadingEventAsync(Reading reading, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending reading with id {Guid} to New Relic", reading.Id);
        
        var data = new
        {
            eventType = "Readings",
            id = reading.Id,
            plotId = reading.PlotId,
            plotName = reading.PlotName,
            soilMoisture = reading.SoilMoisture,
            temperature = reading.Temperature,
            precipitation = reading.Precipitation,
            createdIn = reading.CreatedIn
        };
        var content = new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8, 
            "application/json"
        );
        var response = await httpClient.PostAsync($"{options.Value.AccountId}/events", content, cancellationToken: cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to publish reading event with id {Guid} to New Relic. Status code: {StatusCode}", reading.Id, response.StatusCode);
            throw new Exception("Failed to publish reading event to New Relic");
        }

        logger.LogInformation("Reading event with id {Guid} has been published to New Relic", reading.Id);
    }
}