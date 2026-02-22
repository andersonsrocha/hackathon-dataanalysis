using HackathonDataAnalysis.Domain.Interfaces;
using HackathonDataAnalysis.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HackathonDataAnalysis.Data.Repositories;

public class ReadingRepository(HackathonDataAnalysisContext context) : Repository<Reading>(context), IReadingRepository
{
    public async Task<IEnumerable<Reading>> FindByPlotAndSinceAsync(Guid plotId, DateTime since, CancellationToken cancellationToken)
        => await _dbSet.AsNoTracking().Where(r => r.Active && r.Date >= since && r.PlotId == plotId).ToListAsync(cancellationToken);
}