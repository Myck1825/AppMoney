using AppModey.Database.Constants;
using AppMoney.Database;
using AppMoney.Database.Entities;
using AppMoney.Respose;
using AppMoney.Respose.CustomException;
using AppMoney.Respose.Enums;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AppModey.Database.Repository
{
    public class ApplicationRepository : RepositoryBase, IApplicationRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<ApplicationRepository> _logger;

        public ApplicationRepository(IDbConnectionFactory connectionFactory,
            ILogger<ApplicationRepository> logger) : base(connectionFactory, logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DbResult<Guid>> AddAsync(Application entity)
            => await BaseInvokeAsync<Guid>(async (dbResult, connection) =>
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var parameters = new DynamicParameters();
            parameters.Add("@client_id", entity.ClientId, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@ip_address", entity.ClientIp, DbType.String, ParameterDirection.Input);
            parameters.Add("@department_address", entity.DepartmentAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@amount", entity.Amount, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@currency", entity.Currency, DbType.String, ParameterDirection.Input);
            parameters.Add("@id", dbType: DbType.Guid, direction: ParameterDirection.Output);
            parameters.Add("@error_code", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@error_message", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);


            Guid id = await connection.ExecuteScalarAsync<Guid>(sql: ApplicationStoredProcedure.RegisterApplication,
                param: parameters, commandType: CommandType.StoredProcedure);

            dbResult.SetSuccess(id);

        }, entity);

        public async Task<DbResult<IReadOnlyList<Application>>> GetByFilterAsync(Guid clientId, string departmentAddress)
            => await BaseInvokeAsync<IReadOnlyList<Application>>(async (dbResult, connection) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("client_id", clientId, DbType.Guid, ParameterDirection.Input);
            parameters.Add("department_address", departmentAddress, DbType.String, ParameterDirection.Input);

            var applicationList = await connection.QueryAsync<Application>
                (ApplicationStoredProcedure.GetApplicationByCliendIdAndDepartementAddress, parameters, commandType: CommandType.StoredProcedure);

            dbResult.SetSuccess(applicationList?.ToList() ?? []);
        });

        public async Task<DbResult<Application>> GetByIdAsync(Guid id)
            => await BaseInvokeAsync<Application>(async (dbResult, connection) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Guid, ParameterDirection.Input);

            var application = await connection.QueryFirstOrDefaultAsync<Application>
                (ApplicationStoredProcedure.GetApplicationByID, parameters, commandType: CommandType.StoredProcedure);

            if (application == null)
            {
                throw new NotFoundException();
            }
            else 
                dbResult.SetSuccess(application!);
        });
    }
}
