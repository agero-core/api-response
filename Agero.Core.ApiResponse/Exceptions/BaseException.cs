using Agero.Core.Checker;
using System;
using System.Net;

namespace Agero.Core.ApiResponse.Exceptions
{
    /// <summary>Base exception</summary>
    public abstract class BaseException : Exception
    {
        /// <summary>Constructor</summary>
        /// <param name="message">Message, which is added to HTTP response (required)</param>
        /// <param name="code">Code, which is added to HTTP response (required)</param>
        /// <param name="additionalData">Additional data, which is added to log (optional)</param>
        /// <param name="innerException">Inner exception (optional)</param>
        protected BaseException(string message, string code, object additionalData = null, Exception innerException = null)
            : base(message, innerException)
        {
            Check.ArgumentIsNullOrWhiteSpace(message, "message");
            Check.ArgumentIsNullOrWhiteSpace(code, "code");

            Code = code;
            AdditionalData = additionalData;
        }

        /// <summary>Code, which is added to HTTP response</summary>
        public string Code { get; }

        /// <summary>Additional data, which is added to log</summary>
        public object AdditionalData { get; }

        /// <summary>HTTP status code of response</summary>
        public abstract HttpStatusCode HttpStatusCode { get; }
    }
}
