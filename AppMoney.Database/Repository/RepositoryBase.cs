using AppMoney.Database.Validation;
using AppMoney.Respose;
using AppMoney.Respose.CustomException;
using AppMoney.Respose.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;

namespace AppModey.Database.Repository
{
    public abstract class RepositoryBase
    {
        protected const string ModelIsNotValidErrorMessage = "Model Is Not Valid.";
        protected readonly IDbConnectionFactory _context;
        private readonly ILogger<RepositoryBase> _logger;


        public RepositoryBase(IDbConnectionFactory context,
            ILogger<RepositoryBase> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(logger));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DbResult<T>> BaseInvokeAsync<T>(Func<DbResult<T>, IDbConnection, Task> func, object? request = null)
        {
            DbResult<T> dbResult = new DbResult<T>();
            using (IDbConnection connection = _context.CreateConnection())
            {
                try
                {
                    List<ValidationResult> checkModelResult = CheckRequest(request);

                    if (checkModelResult.Any())
                    {
                        dbResult.SetError(ModelIsNotValidErrorMessage, checkModelResult.Select(x => new Error { Key = x.MemberNames.FirstOrDefault(), Message = x.ErrorMessage }));
                    }
                    else
                    {
                        await func(dbResult, connection);
                    }
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, ex.Message);

                    var errorCode = ExtractErrorCode(ex.Message);

                    if (errorCode != null && errorCode != 0)
                    {
                        string errorMessage = ExtractMessage(ex.Message);
                        Exception? exception = null;
                        Code code;
                        switch (errorCode)
                        {
                            case (int)Code.BadRequest:
                                exception = new RecordIsIncorrectException(errorMessage);
                                code = Code.BadRequest;
                                break;
                            case (int)Code.Conflict:
                                exception = new AlreadyExistEcxeption(errorMessage);
                                code = Code.Conflict;
                                break;
                            case (int)Code.SQLDATA:
                                exception = new SqlDataException(errorMessage);
                                code = Code.SQLDATA;
                                break;
                            default:
                                exception = ex;
                                code = Code.Internal;
                                break;
                        }
                        dbResult.SetError(errorMessage, code, exception);
                    }
                    else
                        dbResult.SetError(ex.Message, Code.SQLDATA, ex);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    dbResult.SetError(ex.Message, Code.SQLDATA, ex);
                }
            }
            return dbResult;
        }

        #region -- Private helper --
        public int? ExtractErrorCode(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return null;

            var match = Regex.Match(message, @"Error Code:\s*(\d+)");
            
            int code;

            return match.Success ? 
                int.TryParse(match.Groups[1].Value, out code) ? code : null 
                : null;
        }

        public string ExtractMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return string.Empty;

            var match = Regex.Match(message, @"Message:\s*Message:\s*(.+?)\s*Error Code:");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private List<ValidationResult> CheckRequest(object? request)
        {
            request = request ?? new { };

            var validationResultList = new List<ValidationResult>();
            Validator.TryValidateObject(request, new ValidationContext(request), validationResultList, true);

            return SelectMany(validationResultList);
        }

        private List<ValidationResult> SelectMany(IEnumerable<ValidationResult> validationResultList)
        {
            var list = new List<ValidationResult>();

            list.AddRange(validationResultList.Select(x =>
            {
                if (x is CompositeValidationResult cvr)
                {
                    list.AddRange(SelectMany(cvr.Results));
                }
                return x;
            }));

            return list;
        }

        #endregion
    }
}
