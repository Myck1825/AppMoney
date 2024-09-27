using AppModey.Database.Repository;

namespace AppModey.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IApplicationRepository applicationRepository)
        {
            Applications = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
        }
        public IApplicationRepository Applications { get; }
    }
}
