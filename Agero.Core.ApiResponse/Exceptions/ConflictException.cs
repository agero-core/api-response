using Agero.Core.ApiResponse.Models;
using System;
using System.Net;

namespace Agero.Core.ApiResponse.Exceptions
{
    /// <summary>Exception which will lead to 'Conflict' (409) HTTP response</summary>
    public class ConflictException : BaseException
    {
        /// <summary>Constructor</summary>
        /// <param name="message">Message, which is added to HTTP response (required)</param>
        /// <param name="code">Code, which is added to HTTP response (required)</param>
        /// <param name="additionalData">Additional data, which is added to log (optional)</param>
        /// <param name="innerException">Inner exception (optional)</param>
        public ConflictException(string message, string code = ResponseCode.DATA_LOCKED_FOR_UPDATE, object additionalData = null, Exception innerException = null) 
            : base(message, code, additionalData, innerException)
        {
        }

        /// <summary>HTTP status code of response</summary>
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.Conflict;
    }
}
