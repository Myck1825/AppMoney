namespace AppMoney.Database.SqlCommandFactory
{
    public interface ISqlComandFactory
    {
        public IApplicationSqlCommand ApplicationSqlCommand { get; }
    }
}
