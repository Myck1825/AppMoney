using AppMoney.Database.Enums;

namespace AppMoney.Database.Options
{
    public class DbOptions
    {
        public required string ConnectionString {  get; set; }
        public DatabaseType DbType { get; set; }
    }
}
