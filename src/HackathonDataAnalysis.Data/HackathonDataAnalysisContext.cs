using Microsoft.EntityFrameworkCore;

namespace HackathonDataAnalysis.Data;

public abstract class HackathonDataAnalysisContext<TContext>(DbContextOptions<TContext> options) : DbContext(options) where TContext : DbContext;