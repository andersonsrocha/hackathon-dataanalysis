using HackathonDataAnalysis.Domain.Models;

namespace HackathonDataAnalysis.Domain.Interfaces;

public interface IReadingRepository : IRepository<Reading>
{
    Task<IEnumerable<Reading>> FindByPlotAsync(Guid plotId, CancellationToken cancellationToken);
    Task<IEnumerable<Reading>> FindByPlotAndSinceAsync(Guid plotId, DateTime since, CancellationToken cancellationToken);
}