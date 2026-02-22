using HackathonDataAnalysis.Domain.Models;

namespace HackathonDataAnalysis.Domain.Interfaces;

public interface IRuleRepository : IRepository<Rule>
{
    Task<IEnumerable<Rule>> FindByPlotAsync(Guid plotId, CancellationToken cancellationToken);
}