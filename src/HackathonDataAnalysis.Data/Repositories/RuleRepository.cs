using HackathonDataAnalysis.Domain.Interfaces;
using HackathonDataAnalysis.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HackathonDataAnalysis.Data.Repositories;

public class RuleRepository(HackathonDataAnalysisContext context) : Repository<Rule>(context), IRuleRepository
{
    public async Task<IEnumerable<Rule>> FindByPlotAsync(Guid plotId, CancellationToken cancellationToken)
        => await _dbSet.AsNoTracking().Where(r => r.Active && r.PlotId == plotId).ToListAsync(cancellationToken);
}