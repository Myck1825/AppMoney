namespace AppMoney.Database.SqlCommandFactory
{
    public interface IApplicationSqlCommand
    {
        string GetById();
        string GetByFilter();

        string RegisterApplicationStoredProcedureName { get; }
    }
}
