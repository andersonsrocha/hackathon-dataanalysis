using System.Net.Http.Json;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Plots.Interfaces;
using Microsoft.Extensions.Logging;

namespace HackathonDataAnalysis.Plots.Services;

public class PlotService(HttpClient httpClient, ILogger<PlotService> logger) : IPlotService
{
    public async Task<PlotDto?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting plot with id {Guid}", id);
        
        var response = await httpClient.GetAsync(id.ToString(), cancellationToken: cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Plot with id {Guid} was not found", id);
            return null;
        }
        
        logger.LogInformation("Plot has been retrieved");
        var plotDto = await response.Content.ReadFromJsonAsync<PlotDto>(cancellationToken: cancellationToken);
        return plotDto;
    }
}