namespace SocialNetworkProject.Core.Domain.Interfaces.Generic
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> AddAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllQuery();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> UpdateAsync(int id, TEntity entity);
    }
}
