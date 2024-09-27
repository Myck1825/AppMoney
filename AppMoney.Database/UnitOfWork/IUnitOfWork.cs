using AppModey.Database.Repository;

namespace AppModey.Database.UnitOfWork
{
    public interface IUnitOfWork
    {
        IApplicationRepository Applications { get; }
    }
}
