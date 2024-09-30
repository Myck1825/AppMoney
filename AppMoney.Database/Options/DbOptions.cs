using AppMoney.Database.Enums;

namespace AppMoney.Database.Options
{
    public class DbOptions
    {
        public required ConnectionString ConnectionString {  get; set; }
        public DatabaseType DbType { get; set; }
    }

    public class ConnectionString
    {
        public required string MSConnectionString { get; set; }
        public string PostgreConnectionString { get; set; }
    }
}
