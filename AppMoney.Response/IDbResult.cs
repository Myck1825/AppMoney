using AppMoney.Respose.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMoney.Respose
{
    public interface IDbResult<T> where T : class
    {
        /// <summary>
        /// Gets a value indicating whether this is success.
        /// </summary>
        /// <value><c>true</c> if is success; otherwise, <c>false</c>.</value>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception? Exception { get; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        public Code Code { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string? Message { get; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public T? Result { get; }

        void SetSuccess();
        void SetSuccess(T result);
        void SetError(string message, Code code = Code.Internal, Exception? ex = null);
    }
}
