using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repositoy<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
    }
}