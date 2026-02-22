using Microsoft.EntityFrameworkCore;
using HackathonDataAnalysis.Domain.Interfaces;
using HackathonDataAnalysis.Domain.Models;

namespace HackathonDataAnalysis.Data.Repositories;

public abstract class Repository<TEntity>(HackathonDataAnalysisContext context) : IRepository<TEntity> where TEntity : Entity
{
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    
    public TEntity? Find(Guid id)
        => _dbSet.AsNoTracking().SingleOrDefault(x => x.Id == id);

    public async Task<TEntity?> FindAsync(Guid id, CancellationToken cancellationToken)
        => await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public IEnumerable<TEntity> Find()
        => _dbSet.AsNoTracking().ToList();

    public async Task<IEnumerable<TEntity>> FindAsync(CancellationToken cancellationToken)
        => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public void Add(TEntity entity)
        => _dbSet.Add(entity);

    public void Update(TEntity entity)
        => _dbSet.Update(entity);

    public void Delete(TEntity entity)
        => _dbSet.Remove(entity);
}