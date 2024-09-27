using System.Data;

namespace AppModey.Database
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateConnection();
    }
}
