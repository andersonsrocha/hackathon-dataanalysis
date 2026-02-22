using HackathonDataAnalysis.Domain.Dto;

namespace HackathonDataAnalysis.Plots.Interfaces;

public interface IPlotService
{
    Task<PlotDto?> FindAsync(Guid id, CancellationToken cancellationToken);
}