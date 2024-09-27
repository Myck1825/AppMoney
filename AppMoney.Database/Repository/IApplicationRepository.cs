using AppMoney.Database.Entities;
using AppMoney.Respose;

namespace AppModey.Database.Repository
{
    public interface IApplicationRepository : IRepository<Application>
    {
        Task<DbResult<IReadOnlyList<Application>>> GetByFilterAsync(Guid clientId, string departmentAddress);
    }
}
