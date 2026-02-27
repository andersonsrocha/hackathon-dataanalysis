using HackathonDataAnalysis.Domain.Interfaces;

namespace HackathonDataAnalysis.Data;

public sealed class UnitOfWork(HackathonDataAnalysisMongoContext mongoContext, HackathonDataAnalysisSqlContext sqlContext) : IUnitOfWork
{
    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
    {
        var tasks = new List<Task<int>>();

        if (mongoContext.ChangeTracker.HasChanges())
            tasks.Add(mongoContext.SaveChangesAsync(cancellationToken));

        if (sqlContext.ChangeTracker.HasChanges())
            tasks.Add(sqlContext.SaveChangesAsync(cancellationToken));

        return (await Task.WhenAll(tasks)).All(t => t > 0);
    }
}