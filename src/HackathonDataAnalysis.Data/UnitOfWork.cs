using HackathonDataAnalysis.Domain.Interfaces;

namespace HackathonDataAnalysis.Data;

public sealed class UnitOfWork(HackathonDataAnalysisContext context) : IUnitOfWork
{
    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken) > 0;
}