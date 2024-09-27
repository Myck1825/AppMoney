using AppMoney.Respose.CustomException;
using AppMoney.Respose.Enums;

namespace AppMoney.Respose
{
    public class DbResult<T>
    {
        private bool _isResultSet;

        public bool IsSuccess { get; private set; }

        public Exception? Exception { get; private set; }

        public Code Code { get; private set; }

        public string? Message { get; private set; }

        public T? Result { get; private set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public IEnumerable<Error>? Errors { get; set; }

        /// <summary>
        /// Create new isntance
        /// </summary>
        public DbResult() 
        {
            ResultSet(false);
        }

        /// <summary>
        /// Sets the success.
        /// </summary>
        public void SetSuccess()
        {
            CheckResult();

            SetResult(true, Code.Ok, default);
        }

        /// <summary>
        /// Sets the success.
        /// </summary>
        /// <param name="result">Result.</param>
        public void SetSuccess(T result)
        {
            CheckResult();

            SetResult(true, Code.Ok, result);
        }

        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="erorrs">Erorrs.</param>
        /// <param name="ex">Ex.</param>
        public void SetError(string message, IEnumerable<Error> erorrs, Code code = Code.Internal, Exception? ex = null)
        {
            CheckResult();

            SetResult(false, code, default, erorrs, message, ex);
        }

        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="ex">Ex.</param>
        public void SetError(string message, Code code = Code.Internal, Exception? ex = null)
        {
            CheckResult();

            SetResult(false, code, default, null, message, ex);
        }

        #region -- Private helpers --

        private void CheckResult()
        {
            if (_isResultSet)
            {
                throw new ResultAlreadySetDbResultException();
            }
        }

        private void ResultSet(bool isResultSet)
        {
            _isResultSet = isResultSet;
        }

        private void SetResult(bool isSuccess, Code code, T? result, IEnumerable<Error>? erorrs = null, string message = "", Exception? ex = null)
        {
            var finishTime = DateTime.UtcNow;
            IsSuccess = isSuccess;
            Exception = ex;
            Result = result;
            Code = code;
            Errors = erorrs;
            Message = message;
            ResultSet(true);
        }

        #endregion
    }
}
