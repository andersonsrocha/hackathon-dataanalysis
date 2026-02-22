using Microsoft.EntityFrameworkCore;
using HackathonDataAnalysis.Domain.Models;
using MongoDB.EntityFrameworkCore.Extensions;

namespace HackathonDataAnalysis.Data;

public class HackathonDataAnalysisContext(DbContextOptions<HackathonDataAnalysisContext> options) : DbContext(options)
{
    public DbSet<Reading> Readings { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(HackathonDataAnalysisContext).Assembly);
        builder.Entity<Reading>().ToCollection("readings");
    }
}