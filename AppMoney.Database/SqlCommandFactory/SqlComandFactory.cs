using AppMoney.Database.Enums;
using AppMoney.Database.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMoney.Database.SqlCommandFactory
{
    public class SqlComandFactory : ISqlComandFactory
    {
        private DbOptions _options;

        public SqlComandFactory(IOptions<DbOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IApplicationSqlCommand ApplicationSqlCommand
        {
            get
            {
                switch (_options.DbType)
                {
                    case DatabaseType.MSSQL:
                        return new ApplicationMsCommand();
                    case DatabaseType.PostgreSQL:
                        return new ApplicationPsCommand();
                    default:
                        throw new NotSupportedException();
                }
            }
        }

       
    }
}
