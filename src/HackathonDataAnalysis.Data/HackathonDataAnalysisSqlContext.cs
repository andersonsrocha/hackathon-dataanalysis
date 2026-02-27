using Microsoft.EntityFrameworkCore;
using HackathonDataAnalysis.Domain.Models;

namespace HackathonDataAnalysis.Data;

public class HackathonDataAnalysisSqlContext(DbContextOptions<HackathonDataAnalysisSqlContext> options) : HackathonDataAnalysisContext<HackathonDataAnalysisSqlContext>(options)
{
    public DbSet<Rule> Rules { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(HackathonDataAnalysisSqlContext).Assembly);
    }
}