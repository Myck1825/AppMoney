using AppMoney.Respose;

namespace AppModey.Database.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<DbResult<T>> GetByIdAsync(Guid id);
        Task<DbResult<Guid>> AddAsync(T entity);
    }
}
