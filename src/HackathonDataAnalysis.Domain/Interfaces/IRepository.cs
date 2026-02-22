using HackathonDataAnalysis.Domain.Models;

namespace HackathonDataAnalysis.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
    TEntity? Find(Guid id);
    Task<TEntity?> FindAsync(Guid id, CancellationToken cancellationToken);
    IEnumerable<TEntity> Find();
    Task<IEnumerable<TEntity>> FindAsync(CancellationToken cancellationToken);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}