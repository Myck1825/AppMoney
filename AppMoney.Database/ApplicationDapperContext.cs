using AppModey.Database;
using AppMoney.Database.Enums;
using AppMoney.Database.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace AppMoney.Database
{
    public class ApplicationDapperContext : IDbConnectionFactory
    {
        private readonly DbOptions _options;
        public ApplicationDapperContext(IOptions<DbOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IDbConnection CreateConnection()
        {
            IDbConnection connection;

            switch (_options.DbType)
            {
                case DatabaseType.MSSQL:
                    connection = new SqlConnection(_options.ConnectionString.MSConnectionString);
                    break;
                case DatabaseType.PostgreSQL:
                    connection = new NpgsqlConnection(_options.ConnectionString.PostgreConnectionString);
                    break;
                default:
                    throw new ArgumentException($"{nameof(_options.DbType)} not found");
            }
            
            return connection;
        }
    }
}
